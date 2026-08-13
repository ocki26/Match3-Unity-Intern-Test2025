using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public event Action<eStateGame> StateChangedAction = delegate { };

    public enum ePlayMode
    {
        MANUAL,        // Player taps manually
        AUTOPLAY_WIN,  // AI plays to win
        AUTO_LOSE,     // AI plays to lose
        TIME_ATTACK    // Time Attack mode: 1-minute timer, can return items to board
    }

    public enum eStateGame
    {
        SETUP,
        MAIN_MENU,
        GAME_STARTED,
        PAUSE,
        GAME_OVER,
    }

    public eStateGame State { get; private set; }
    public ePlayMode CurrentPlayMode { get; private set; }
    public bool IsGameWon { get; private set; }

    private const float TIME_ATTACK_DURATION = 60f; // 1 minute countdown for Time Attack mode
    private float m_timeRemaining;

    private GameSettings m_gameSettings;
    private BoardController m_boardController;
    private AutoplayController m_autoplayController;
    private UIMainManager m_uiMenu;

    private void Awake()
    {
        State = eStateGame.SETUP;
        m_gameSettings = Resources.Load<GameSettings>(Constants.GAME_SETTINGS_PATH);

        m_uiMenu = FindObjectOfType<UIMainManager>();
        m_uiMenu.Setup(this);

        m_autoplayController = gameObject.AddComponent<AutoplayController>();
    }

    void Start()
    {
        SetState(eStateGame.MAIN_MENU);
    }

    void Update()
    {
        // Handle Time Attack 1-minute countdown
        if (State == eStateGame.GAME_STARTED && CurrentPlayMode == ePlayMode.TIME_ATTACK)
        {
            m_timeRemaining -= Time.deltaTime;
            m_uiMenu.UpdateTimerDisplay(m_timeRemaining);

            // Lose condition for Time Attack: Time runs out
            if (m_timeRemaining <= 0f)
            {
                m_timeRemaining = 0f;
                m_uiMenu.UpdateTimerDisplay(0f);
                OnGameEnded(false);
            }
        }
    }

    internal void SetState(eStateGame state)
    {
        State = state;
        StateChangedAction(State);

        if (State == eStateGame.PAUSE) DOTween.PauseAll();
        else DOTween.PlayAll();
    }

    /// <summary>
    /// Starts a level under the specified gameplay mode.
    /// </summary>
    public void StartLevel(ePlayMode mode)
    {
        ClearLevel();
        CurrentPlayMode = mode;

        m_boardController = new GameObject("BoardController").AddComponent<BoardController>();
        m_boardController.StartGame(this, m_gameSettings);

        // Reset timer if playing in Time Attack mode
        if (mode == ePlayMode.TIME_ATTACK)
        {
            m_timeRemaining = TIME_ATTACK_DURATION;
            m_uiMenu.UpdateTimerDisplay(m_timeRemaining);
        }
        else
        {
            m_uiMenu.ResetTimerDisplay();
        }

        SetState(eStateGame.GAME_STARTED);

        // Start AI autoplay if requested
        if (mode == ePlayMode.AUTOPLAY_WIN)
        {
            m_autoplayController.StartAutoplay(m_boardController, true);
        }
        else if (mode == ePlayMode.AUTO_LOSE)
        {
            m_autoplayController.StartAutoplay(m_boardController, false);
        }
    }

    /// <summary>
    /// Toggles autoplay mode on/off in real-time during gameplay.
    /// </summary>
    public bool ToggleAutoplay()
    {
        if (State != eStateGame.GAME_STARTED || m_boardController == null) return false;

        if (CurrentPlayMode == ePlayMode.AUTOPLAY_WIN)
        {
            // Switch back to manual play
            CurrentPlayMode = ePlayMode.MANUAL;
            m_autoplayController.StopAutoplay();
            return false;
        }
        else
        {
            // Switch to autoplay
            CurrentPlayMode = ePlayMode.AUTOPLAY_WIN;
            m_autoplayController.StartAutoplay(m_boardController, true);
            return true;
        }
    }

    /// <summary>
    /// Handles game ending (win or lose).
    /// </summary>
    public void OnGameEnded(bool isWin)
    {
        IsGameWon = isWin;
        if (m_autoplayController) m_autoplayController.StopAutoplay();
        SetState(eStateGame.GAME_OVER);
    }

    /// <summary>
    /// Cleans up board controller and autoplay components.
    /// </summary>
    internal void ClearLevel()
    {
        if (m_autoplayController) m_autoplayController.StopAutoplay();

        if (m_boardController)
        {
            m_boardController.Clear();
            Destroy(m_boardController.gameObject);
            m_boardController = null;
        }
    }
}
