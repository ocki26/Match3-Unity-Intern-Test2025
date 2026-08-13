using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIMainManager : MonoBehaviour
{
    private IMenu[] m_menuList;
    private GameManager m_gameManager;

    private void Awake()
    {
        m_menuList = GetComponentsInChildren<IMenu>(true);
    }

    void Start()
    {
        for (int i = 0; i < m_menuList.Length; i++)
        {
            m_menuList[i].Setup(this);
        }
    }

    void Update()
    {
        // Toggle pause on Escape key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (m_gameManager.State == GameManager.eStateGame.GAME_STARTED)
            {
                m_gameManager.SetState(GameManager.eStateGame.PAUSE);
            }
            else if (m_gameManager.State == GameManager.eStateGame.PAUSE)
            {
                m_gameManager.SetState(GameManager.eStateGame.GAME_STARTED);
            }
        }
    }

    /// <summary>
    /// Connects UIMainManager with GameManager and listens to state changes.
    /// </summary>
    internal void Setup(GameManager gameManager)
    {
        m_gameManager = gameManager;
        m_gameManager.StateChangedAction += OnGameStateChange;
    }

    private void OnGameStateChange(GameManager.eStateGame state)
    {
        switch (state)
        {
            case GameManager.eStateGame.SETUP:
                break;
            case GameManager.eStateGame.MAIN_MENU:
                ShowMenu<UIPanelMain>();
                break;
            case GameManager.eStateGame.GAME_STARTED:
                ShowMenu<UIPanelGame>();
                break;
            case GameManager.eStateGame.PAUSE:
                ShowMenu<UIPanelPause>();
                break;
            case GameManager.eStateGame.GAME_OVER:
                ShowMenu<UIPanelGameOver>();
                break;
        }
    }

    private void ShowMenu<T>() where T : IMenu
    {
        for (int i = 0; i < m_menuList.Length; i++)
        {
            IMenu menu = m_menuList[i];
            if (menu is T)
            {
                menu.Show();
            }
            else
            {
                menu.Hide();
            }
        }
    }

    internal void ShowMainMenu()
    {
        m_gameManager.ClearLevel();
        m_gameManager.SetState(GameManager.eStateGame.MAIN_MENU);
    }

    internal void ShowPauseMenu()
    {
        m_gameManager.SetState(GameManager.eStateGame.PAUSE);
    }

    internal void ShowGameMenu()
    {
        m_gameManager.SetState(GameManager.eStateGame.GAME_STARTED);
    }

    /// <summary>
    /// Updates timer display on UIPanelGame.
    /// </summary>
    internal void UpdateTimerDisplay(float secondsRemaining)
    {
        UIPanelGame gamePanel = m_menuList.OfType<UIPanelGame>().FirstOrDefault();
        if (gamePanel != null)
        {
            gamePanel.UpdateTimerText(secondsRemaining);
        }
    }

    /// <summary>
    /// Resets timer display on UIPanelGame.
    /// </summary>
    internal void ResetTimerDisplay()
    {
        UIPanelGame gamePanel = m_menuList.OfType<UIPanelGame>().FirstOrDefault();
        if (gamePanel != null)
        {
            gamePanel.ResetConditionView();
        }
    }

    /// <summary>
    /// Toggles in-game autoplay on and off.
    /// </summary>
    internal bool ToggleInGameAutoplay()
    {
        return m_gameManager != null && m_gameManager.ToggleAutoplay();
    }

    // Play Mode triggers
    internal void StartManualPlay() => m_gameManager.StartLevel(GameManager.ePlayMode.MANUAL);
    internal void StartAutoplayWin() => m_gameManager.StartLevel(GameManager.ePlayMode.AUTOPLAY_WIN);
    internal void StartAutoLose() => m_gameManager.StartLevel(GameManager.ePlayMode.AUTO_LOSE);
    internal void StartTimeAttack() => m_gameManager.StartLevel(GameManager.ePlayMode.TIME_ATTACK);
}
