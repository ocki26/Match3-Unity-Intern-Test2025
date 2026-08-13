using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPanelGame : MonoBehaviour, IMenu
{
    public Text LevelConditionView;

    [SerializeField] private Button btnPause;
    [SerializeField] private Button btnToggleAutoplay; // In-game toggle button for Autoplay

    private UIMainManager m_mngr;
    private Text m_txtAutoplay;
    private bool m_isAutoplayActive = false;

    private void Awake()
    {
        if (btnPause) btnPause.onClick.AddListener(OnClickPause);

        // Dynamically create in-game Autoplay toggle button if not present in Scene
        EnsureAutoplayButtonExists();
    }

    private void EnsureAutoplayButtonExists()
    {
        if (btnToggleAutoplay == null && btnPause != null)
        {
            GameObject newObj = Instantiate(btnPause.gameObject, btnPause.transform.parent);
            newObj.name = "Btn_ToggleAutoplay";
            newObj.SetActive(true);

            RectTransform rt = newObj.GetComponent<RectTransform>();
            RectTransform pauseRt = btnPause.GetComponent<RectTransform>();

            if (rt != null && pauseRt != null)
            {
                // Position neatly below Pause button with clean dimensions
                rt.anchoredPosition = pauseRt.anchoredPosition + new Vector2(0, -60f);
                rt.sizeDelta = new Vector2(130f, 42f);
            }

            btnToggleAutoplay = newObj.GetComponent<Button>();
            btnToggleAutoplay.onClick.RemoveAllListeners();

            m_txtAutoplay = btnToggleAutoplay.GetComponentInChildren<Text>();
            if (m_txtAutoplay != null)
            {
                RectTransform textRt = m_txtAutoplay.GetComponent<RectTransform>();
                if (textRt != null)
                {
                    textRt.anchorMin = Vector2.zero;
                    textRt.anchorMax = Vector2.one;
                    textRt.offsetMin = Vector2.zero;
                    textRt.offsetMax = Vector2.zero;
                }

                m_txtAutoplay.text = "AUTO: OFF";
                m_txtAutoplay.fontSize = 14;
                m_txtAutoplay.fontStyle = FontStyle.Bold;
                m_txtAutoplay.alignment = TextAnchor.MiddleCenter;
                m_txtAutoplay.horizontalOverflow = HorizontalWrapMode.Overflow;
                m_txtAutoplay.verticalOverflow = VerticalWrapMode.Overflow;
                m_txtAutoplay.resizeTextForBestFit = false;
                m_txtAutoplay.color = new Color(0.15f, 0.15f, 0.15f, 1f);
            }
        }
        else if (btnToggleAutoplay != null)
        {
            m_txtAutoplay = btnToggleAutoplay.GetComponentInChildren<Text>();
        }

        if (btnToggleAutoplay != null)
        {
            btnToggleAutoplay.onClick.AddListener(OnClickToggleAutoplay);
        }
    }

    private void OnClickPause()
    {
        m_mngr.ShowPauseMenu();
    }

    private void OnClickToggleAutoplay()
    {
        if (m_mngr != null)
        {
            m_isAutoplayActive = m_mngr.ToggleInGameAutoplay();
            UpdateAutoplayButtonVisual();
        }
    }

    private void UpdateAutoplayButtonVisual()
    {
        if (m_txtAutoplay != null)
        {
            m_txtAutoplay.text = m_isAutoplayActive ? "AUTO: ON 🟢" : "AUTO: OFF ⚪";
        }
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
        m_isAutoplayActive = false;
        UpdateAutoplayButtonVisual();
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }
}
