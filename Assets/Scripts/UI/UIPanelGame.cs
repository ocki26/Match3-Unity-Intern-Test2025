using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPanelGame : MonoBehaviour, IMenu
{
    public Text LevelConditionView;

    [SerializeField] private Button btnPause;

    private UIMainManager m_mngr;

    private void Awake()
    {
        if (btnPause) btnPause.onClick.AddListener(OnClickPause);
    }

    private void OnClickPause()
    {
        m_mngr.ShowPauseMenu();
    }

    public void Setup(UIMainManager mngr)
    {
        m_mngr = mngr;
    }

    /// <summary>
    /// Updates the timer countdown display (Time Attack mode).
    /// </summary>
    public void UpdateTimerText(float secondsRemaining)
    {
        if (LevelConditionView != null)
        {
            int minutes = Mathf.FloorToInt(secondsRemaining / 60F);
            int seconds = Mathf.FloorToInt(secondsRemaining - minutes * 60);
            LevelConditionView.text = string.Format("TIME: {0:0}:{1:00}", minutes, seconds);
        }
    }

    /// <summary>
    /// Clears or resets the condition view when not in Time Attack mode.
    /// </summary>
    public void ResetConditionView()
    {
        if (LevelConditionView != null)
        {
            LevelConditionView.text = string.Empty;
        }
    }

    public void Show()
    {
        this.gameObject.SetActive(true);
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }
}
