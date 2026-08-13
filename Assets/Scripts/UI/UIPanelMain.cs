using UnityEngine;
using UnityEngine.UI;

public class UIPanelMain : MonoBehaviour, IMenu
{
    [Header("Mode Buttons")]
    [SerializeField] private Button btnPlay;        // Manual Play button
    [SerializeField] private Button btnAutoplay;    // Autoplay (Win) button
    [SerializeField] private Button btnAutoLose;    // Auto Lose button
    [SerializeField] private Button btnTimeAttack;  // Time Attack Mode button (Task 3 Requirement)

    // Backward compatibility with legacy buttons in Scene
    [SerializeField] private Button btnMoves;
    [SerializeField] private Button btnTimer;

    private UIMainManager m_mngr;

    private void Awake()
    {
        // Bind primary buttons
        if (btnPlay) btnPlay.onClick.AddListener(() => m_mngr.StartManualPlay());
        if (btnAutoplay) btnAutoplay.onClick.AddListener(() => m_mngr.StartAutoplayWin());
        if (btnAutoLose) btnAutoLose.onClick.AddListener(() => m_mngr.StartAutoLose());
        if (btnTimeAttack) btnTimeAttack.onClick.AddListener(() => m_mngr.StartTimeAttack());

        // Bind legacy buttons if scene uses them
        if (btnMoves && btnPlay == null) btnMoves.onClick.AddListener(() => m_mngr.StartManualPlay());
        if (btnTimer && btnTimeAttack == null) btnTimer.onClick.AddListener(() => m_mngr.StartTimeAttack());
    }

    private void OnDestroy()
    {
        if (btnPlay) btnPlay.onClick.RemoveAllListeners();
        if (btnAutoplay) btnAutoplay.onClick.RemoveAllListeners();
        if (btnAutoLose) btnAutoLose.onClick.RemoveAllListeners();
        if (btnTimeAttack) btnTimeAttack.onClick.RemoveAllListeners();
        if (btnMoves) btnMoves.onClick.RemoveAllListeners();
        if (btnTimer) btnTimer.onClick.RemoveAllListeners();
    }

    public void Setup(UIMainManager mngr) => m_mngr = mngr;
    public void Show() => gameObject.SetActive(true);
    public void Hide() => gameObject.SetActive(false);
}
