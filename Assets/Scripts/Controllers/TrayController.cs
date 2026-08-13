using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

public class TrayController : MonoBehaviour
{
    // Events fired when the game ends
    public event Action OnGameWin = delegate { };
    public event Action OnGameLose = delegate { };

    private const int MAX_CAPACITY = 5; // Tray capacity constraint of 5 slots
    private List<Item> m_trayItems = new List<Item>();
    private List<Vector3> m_slotPositions = new List<Vector3>();
    private bool m_isBusy = false;

    // Time Attack mode flag (Task 3: No loss on full tray, can return items to board)
    public bool IsTimeAttackMode { get; set; } = false;

    // Public properties
    public bool CanAddItem => m_trayItems.Count < MAX_CAPACITY && !m_isBusy;
    public List<Item> CurrentItems => m_trayItems;

    /// <summary>
    /// Initializes slot positions and spawns background tiles for the 5 tray slots below the board.
    /// </summary>
    public void Setup(Transform root, float boardBottomY)
    {
        m_trayItems.Clear();
        m_slotPositions.Clear();

        // Calculate bottom Y position for the tray
        float trayY = boardBottomY - 1.2f;
        float spacing = 1.0f;
        float startX = -(MAX_CAPACITY - 1) * 0.5f * spacing;

        GameObject prefabBG = Resources.Load<GameObject>(Constants.PREFAB_CELL_BACKGROUND);

        for (int i = 0; i < MAX_CAPACITY; i++)
        {
            Vector3 slotPos = new Vector3(startX + i * spacing, trayY, 0f);
            m_slotPositions.Add(slotPos);

            // Spawn background slot visuals
            if (prefabBG != null)
            {
                GameObject bg = Instantiate(prefabBG, slotPos, Quaternion.identity, root);
                bg.name = $"TraySlot_{i}";
                bg.transform.localScale = Vector3.one * 0.9f;
            }
        }
    }

    /// <summary>
    /// Moves an item from the board into the next available slot in the tray with smooth animation.
    /// </summary>
    public void AddItem(Item item, Board board, Action onComplete = null)
    {
        if (!CanAddItem || item == null) return;

        m_isBusy = true;
        m_trayItems.Add(item);

        Vector3 targetPos = m_slotPositions[m_trayItems.Count - 1];

        // Smooth movement and scale animation (Task 3 Requirement)
        item.View.DOScale(Vector3.one * 1.15f, 0.12f).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            item.View.DOScale(Vector3.one, 0.13f).SetEase(Ease.InQuad);
        });

        item.View.DOMove(targetPos, 0.25f).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            StartCoroutine(CheckMatch3Coroutine(board, onComplete));
        });
    }

    /// <summary>
    /// Returns a tray item back to its original cell on the board (Time Attack mode requirement).
    /// </summary>
    public bool ReturnItemToBoard(Item item, Action onComplete = null)
    {
        if (m_isBusy || item == null || !m_trayItems.Contains(item)) return false;

        Cell targetCell = item.OriginalCell;
        if (targetCell == null || !targetCell.IsEmpty)
        {
            // If original cell is occupied, return fails
            return false;
        }

        m_isBusy = true;
        m_trayItems.Remove(item);

        // Reassign item to board cell
        targetCell.Assign(item);

        // Animate movement back to board cell
        item.View.DOMove(targetCell.transform.position, 0.25f).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            RearrangeTrayItems();
            m_isBusy = false;
            onComplete?.Invoke();
        });

        return true;
    }

    /// <summary>
    /// Finds which Item in the tray corresponds to a given visual Transform/Collider.
    /// </summary>
    public Item GetItemFromView(Transform viewTransform)
    {
        return m_trayItems.FirstOrDefault(item => item != null && item.View == viewTransform);
    }

    /// <summary>
    /// Checks for 3 identical items in the tray, clears them, and evaluates win/lose conditions.
    /// </summary>
    private IEnumerator CheckMatch3Coroutine(Board board, Action onComplete)
    {
        // Group items by type to check for triplets
        var groups = m_trayItems
            .OfType<NormalItem>()
            .GroupBy(x => x.ItemType)
            .FirstOrDefault(g => g.Count() >= 3);

        if (groups != null)
        {
            yield return new WaitForSeconds(0.1f);

            List<NormalItem> matchedItems = groups.Take(3).ToList();

            // Clear the 3 matched items with scale-to-zero animation
            foreach (var matchedItem in matchedItems)
            {
                m_trayItems.Remove(matchedItem);
                matchedItem.ExplodeView();
            }

            yield return new WaitForSeconds(0.25f);

            // Shift remaining items to the left
            RearrangeTrayItems();
            yield return new WaitForSeconds(0.2f);
        }

        m_isBusy = false;
        onComplete?.Invoke();

        // 1. Win condition: Board is completely cleared and tray is empty
        if (board.IsAllCleared() && m_trayItems.Count == 0)
        {
            OnGameWin();
        }
        // 2. Lose condition: In normal mode, lose if tray is full (5 items). In Time Attack mode, no loss on full tray!
        else if (!IsTimeAttackMode && m_trayItems.Count >= MAX_CAPACITY)
        {
            OnGameLose();
        }
    }

    /// <summary>
    /// Shifts remaining items smoothly to their corresponding slot positions in the tray.
    /// </summary>
    private void RearrangeTrayItems()
    {
        for (int i = 0; i < m_trayItems.Count; i++)
        {
            if (m_trayItems[i] != null && m_trayItems[i].View != null)
            {
                m_trayItems[i].View.DOMove(m_slotPositions[i], 0.2f).SetEase(Ease.OutQuad);
            }
        }
    }

    /// <summary>
    /// Cleans up all items in the tray.
    /// </summary>
    public void Clear()
    {
        foreach (var item in m_trayItems)
        {
            if (item != null) item.Clear();
        }
        m_trayItems.Clear();
    }
}
