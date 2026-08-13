using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPanelMain : MonoBehaviour, IMenu
{
    [Header("Mode Buttons")]
    [SerializeField] private Button btnPlay;        // Manual Play button
    [SerializeField] private Button btnAutoplay;    // Autoplay (Win) button
    [SerializeField] private Button btnAutoLose;    // Auto Lose button
    [SerializeField] private Button btnTimeAttack;  // Time Attack Mode button

    // Legacy buttons if present in Scene
    [SerializeField] private Button btnMoves;
    [SerializeField] private Button btnTimer;

    private UIMainManager m_mngr;
    private bool m_buttonsInitialized = false;

    // Compact button dimensions and generous spacing
    private readonly Vector2 BUTTON_SIZE = new Vector2(230f, 46f);
    private const float BUTTON_SPACING = 68f;
    private const float START_Y = 105f;

    private void Awake()
    {
        InitializeButtons();
    }

    /// <summary>
    /// Ensures all 4 mode buttons exist, are resized cleanly, spaced apart, and bound to handlers.
    /// </summary>
    private void InitializeButtons()
    {
        if (m_buttonsInitialized) return;
        m_buttonsInitialized = true;

        Button template = btnPlay ?? btnMoves ?? GetComponentInChildren<Button>(true);

        if (template != null)
        {
            Transform parent = template.transform.parent;

            // 1. Play Button (Manual)
            if (btnPlay == null)
            {
                btnPlay = template;
                btnPlay.name = "Btn_PlayManual";
                ApplyButtonStyling(btnPlay, "PLAY (MANUAL)", new Vector2(0, START_Y));
            }
            else
            {
                ApplyButtonStyling(btnPlay, "PLAY (MANUAL)", new Vector2(0, START_Y));
            }

            // 2. Autoplay (Auto Win) Button
            if (btnAutoplay == null)
            {
                btnAutoplay = CreateButton(template, parent, "Btn_Autoplay", "AUTOPLAY (AUTO WIN)", new Vector2(0, START_Y - BUTTON_SPACING));
            }
            else
            {
                ApplyButtonStyling(btnAutoplay, "AUTOPLAY (AUTO WIN)", new Vector2(0, START_Y - BUTTON_SPACING));
            }

            // 3. Auto Lose Button
            if (btnAutoLose == null)
            {
                btnAutoLose = CreateButton(template, parent, "Btn_AutoLose", "AUTO LOSE", new Vector2(0, START_Y - BUTTON_SPACING * 2));
            }
            else
            {
                ApplyButtonStyling(btnAutoLose, "AUTO LOSE", new Vector2(0, START_Y - BUTTON_SPACING * 2));
            }

            // 4. Time Attack Button
            if (btnTimeAttack == null)
            {
                btnTimeAttack = CreateButton(template, parent, "Btn_TimeAttack", "TIME ATTACK (1 MIN)", new Vector2(0, START_Y - BUTTON_SPACING * 3));
            }
            else
            {
                ApplyButtonStyling(btnTimeAttack, "TIME ATTACK (1 MIN)", new Vector2(0, START_Y - BUTTON_SPACING * 3));
            }
        }

        // Register button click listeners
        if (btnPlay) btnPlay.onClick.AddListener(() => m_mngr.StartManualPlay());
        if (btnAutoplay) btnAutoplay.onClick.AddListener(() => m_mngr.StartAutoplayWin());
        if (btnAutoLose) btnAutoLose.onClick.AddListener(() => m_mngr.StartAutoLose());
        if (btnTimeAttack) btnTimeAttack.onClick.AddListener(() => m_mngr.StartTimeAttack());
    }

    private Button CreateButton(Button template, Transform parent, string objName, string label, Vector2 pos)
    {
        GameObject newGo = Instantiate(template.gameObject, parent);
        newGo.name = objName;
        newGo.SetActive(true);

        Button btn = newGo.GetComponent<Button>();
        btn.onClick.RemoveAllListeners();

        ApplyButtonStyling(btn, label, pos);

        return btn;
    }

    private void ApplyButtonStyling(Button btn, string label, Vector2 pos)
    {
        RectTransform rt = btn.GetComponent<RectTransform>();
        if (rt != null)
        {
            rt.sizeDelta = BUTTON_SIZE;
            rt.anchoredPosition = pos;
        }

        Text txt = btn.GetComponentInChildren<Text>();
        if (txt != null)
        {
            txt.text = label;
            txt.fontSize = 18;
            txt.resizeTextForBestFit = true;
            txt.resizeTextMinSize = 12;
            txt.resizeTextMaxSize = 18;
        }
    }

    public void Setup(UIMainManager mngr)
    {
        m_mngr = mngr;
        InitializeButtons();
    }

    public void Show() => gameObject.SetActive(true);
    public void Hide() => gameObject.SetActive(false);
}
