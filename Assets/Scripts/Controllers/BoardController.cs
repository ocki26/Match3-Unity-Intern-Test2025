using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardController : MonoBehaviour
{
    // Event fired whenever an item move action occurs
    public event Action OnMoveEvent = delegate { };

    private Board m_board;
    private GameManager m_gameManager;
    private TrayController m_trayController;
    private Camera m_cam;
    private bool m_gameOver;

    // Public properties
    public bool IsBusy => m_trayController != null && !m_trayController.CanAddItem;
    public Board Board => m_board;
    public TrayController Tray => m_trayController;

    /// <summary>
    /// Initializes board cells and the bottom 5-slot tray.
    /// </summary>
    public void StartGame(GameManager gameManager, GameSettings gameSettings)
    {
        m_gameManager = gameManager;
        m_gameManager.StateChangedAction += OnGameStateChange;
        m_cam = Camera.main;

        // 1. Create and populate the board with items in triplets
        m_board = new Board(this.transform, gameSettings);
        m_board.Fill();

        // 2. Create the bottom tray below the board
        float bottomY = -gameSettings.BoardSizeY * 0.5f;
        m_trayController = gameObject.AddComponent<TrayController>();
        m_trayController.Setup(this.transform, bottomY);

        // Configure Time Attack mode rules (Task 3 requirement)
        m_trayController.IsTimeAttackMode = (m_gameManager.CurrentPlayMode == GameManager.ePlayMode.TIME_ATTACK);

        // 3. Register win and lose event callbacks
        m_trayController.OnGameWin += () => m_gameManager.OnGameEnded(true);
        m_trayController.OnGameLose += () => m_gameManager.OnGameEnded(false);
    }

    private void OnGameStateChange(GameManager.eStateGame state)
    {
        if (state == GameManager.eStateGame.GAME_OVER)
        {
            m_gameOver = true;
        }
    }

    private void Update()
    {
        if (m_gameOver || m_gameManager == null || m_gameManager.State != GameManager.eStateGame.GAME_STARTED) return;

        // Handle player touch/click input when in Manual Play or Time Attack mode
        bool isManualInput = m_gameManager.CurrentPlayMode == GameManager.ePlayMode.MANUAL ||
                             m_gameManager.CurrentPlayMode == GameManager.ePlayMode.TIME_ATTACK;

        if (isManualInput && Input.GetMouseButtonDown(0))
        {
            HandlePlayerTap();
        }
    }

    /// <summary>
    /// Detects taps on board cells (down movement) or tray items (up movement in Time Attack).
    /// </summary>
    private void HandlePlayerTap()
    {
        Vector2 mousePos = m_cam.ScreenToWorldPoint(Input.mousePosition);

        // 1. In Time Attack mode: Check if player tapped an item in the bottom tray to return it UP to the board
        if (m_gameManager.CurrentPlayMode == GameManager.ePlayMode.TIME_ATTACK)
        {
            Item trayItem = m_trayController.GetItemAtWorldPosition(mousePos);
            if (trayItem != null)
            {
                m_trayController.ReturnItemToBoard(trayItem);
                return;
            }
        }

        // 2. Check if player tapped a board Cell to move item DOWN into the tray
        var hit = Physics2D.Raycast(mousePos, Vector2.zero);
        if (hit.collider != null)
        {
            Cell cell = hit.collider.GetComponent<Cell>();
            if (cell != null && !cell.IsEmpty)
            {
                if (m_trayController.CanAddItem)
                {
                    SelectCell(cell);
                }
            }
        }
    }

    /// <summary>
    /// Transfers an item from a board cell into the bottom tray.
    /// </summary>
    public void SelectCell(Cell cell, Action onComplete = null)
    {
        if (cell == null || cell.IsEmpty || !m_trayController.CanAddItem) return;

        Item item = cell.Item;
        cell.Free(); // Free the board cell

        OnMoveEvent(); // Trigger move event

        m_trayController.AddItem(item, m_board, onComplete);
    }

    /// <summary>
    /// Cleans up board and tray items.
    /// </summary>
    public void Clear()
    {
        if (m_board != null) m_board.Clear();
        if (m_trayController != null) m_trayController.Clear();
    }
}
