using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AutoplayController : MonoBehaviour
{
    private BoardController m_boardController;
    private Coroutine m_autoplayCoroutine;

    /// <summary>
    /// Starts autoplay routine for either winning or losing goal.
    /// </summary>
    public void StartAutoplay(BoardController boardController, bool autoWin)
    {
        m_boardController = boardController;
        StopAutoplay();

        m_autoplayCoroutine = StartCoroutine(autoWin ? AutoWinRoutine() : AutoLoseRoutine());
    }

    /// <summary>
    /// Stops any running autoplay coroutine.
    /// </summary>
    public void StopAutoplay()
    {
        if (m_autoplayCoroutine != null)
        {
            StopCoroutine(m_autoplayCoroutine);
            m_autoplayCoroutine = null;
        }
    }

    /// <summary>
    /// Autoplay AI strategy to WIN: prioritizes completing triplets with 0.5s action delay.
    /// </summary>
    private IEnumerator AutoWinRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);

            // Wait if tray is busy animating
            if (m_boardController.IsBusy) continue;

            List<Cell> activeCells = m_boardController.Board.GetActiveCells();
            if (activeCells.Count == 0) yield break; // All items collected

            List<Item> trayItems = m_boardController.Tray.CurrentItems;
            Cell chosenCell = null;

            // Priority 1: Pick a fish matching a type that already has 2 items in tray (completes triplet immediately)
            var typeWithTwo = trayItems.OfType<NormalItem>()
                .GroupBy(x => x.ItemType)
                .FirstOrDefault(g => g.Count() == 2);

            if (typeWithTwo != null)
            {
                chosenCell = activeCells.FirstOrDefault(c => (c.Item as NormalItem)?.ItemType == typeWithTwo.Key);
            }

            // Priority 2: Pick a fish matching a type that has 1 item in tray
            if (chosenCell == null)
            {
                var typeWithOne = trayItems.OfType<NormalItem>()
                    .GroupBy(x => x.ItemType)
                    .FirstOrDefault(g => g.Count() == 1);

                if (typeWithOne != null)
                {
                    chosenCell = activeCells.FirstOrDefault(c => (c.Item as NormalItem)?.ItemType == typeWithOne.Key);
                }
            }

            // Priority 3: Pick the first available cell
            if (chosenCell == null)
            {
                chosenCell = activeCells[0];
            }

            if (chosenCell != null)
            {
                m_boardController.SelectCell(chosenCell);
            }
        }
    }

    /// <summary>
    /// Autoplay AI strategy to LOSE: selects different fish types to deliberately overflow 5-slot tray without forming triplets.
    /// </summary>
    private IEnumerator AutoLoseRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);

            if (m_boardController.IsBusy) continue;

            List<Cell> activeCells = m_boardController.Board.GetActiveCells();
            if (activeCells.Count == 0) yield break;

            List<Item> trayItems = m_boardController.Tray.CurrentItems;
            HashSet<NormalItem.eNormalType> typesInTray = new HashSet<NormalItem.eNormalType>(
                trayItems.OfType<NormalItem>().Select(x => x.ItemType)
            );

            // Deliberately pick a fish type NOT currently in the tray to avoid forming triplets
            Cell chosenCell = activeCells.FirstOrDefault(c =>
            {
                var nItem = c.Item as NormalItem;
                return nItem != null && !typesInTray.Contains(nItem.ItemType);
            });

            // Fallback to first available cell if all remaining types are in tray
            if (chosenCell == null)
            {
                chosenCell = activeCells[0];
            }

            if (chosenCell != null)
            {
                m_boardController.SelectCell(chosenCell);
            }
        }
    }
}
