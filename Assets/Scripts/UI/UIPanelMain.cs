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

    private void Awake()
    {
        InitializeButtons();
    }

    /// <summary>
    /// Ensures all 4 mode buttons exist, are cleanly positioned, labeled, and bound to handlers.
    /// </summary>
    private void InitializeButtons()
    {
        if (m_buttonsInitialized) return;
        m_buttonsInitialized = true;

        Button template = btnPlay ?? btnMoves ?? GetComponentInChildren<Button>(true);

        if (template != null)
        {
            Transform parent = template.transform.parent;
            RectTransform templateRt = template.GetComponent<RectTransform>();
            Vector2 basePos = templateRt != null ? templateRt.anchoredPosition : Vector2.zero;

            // 1. Play Button
            if (btnPlay == null)
            {
                btnPlay = template;
                btnPlay.name = "Btn_PlayManual";
                SetButtonText(btnPlay, "PLAY (MANUAL)");
                if (templateRt != null) templateRt.anchoredPosition = new Vector2(0, 100);
            }

            // 2. Autoplay (Auto Win) Button
            if (btnAutoplay == null)
            {
                btnAutoplay = CreateButton(template, parent, "Btn_Autoplay", "AUTOPLAY (AUTO WIN)", new Vector2(0, 30));
            }

            // 3. Auto Lose Button
            if (btnAutoLose == null)
            {
                btnAutoLose = CreateButton(template, parent, "Btn_AutoLose", "AUTO LOSE", new Vector2(0, -40));
            }

            // 4. Time Attack Button
            if (btnTimeAttack == null)
            {
                btnTimeAttack = CreateButton(template, parent, "Btn_TimeAttack", "TIME ATTACK (1 MIN)", new Vector2(0, -110));
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

        RectTransform rt = newGo.GetComponent<RectTransform>();
        if (rt != null)
        {
            rt.anchoredPosition = pos;
        }

        Button btn = newGo.GetComponent<Button>();
        btn.onClick.RemoveAllListeners();
        SetButtonText(btn, label);

        return btn;
    }

    private void SetButtonText(Button btn, string text)
    {
        Text txt = btn.GetComponentInChildren<Text>();
        if (txt != null)
        {
            txt.text = text;
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
