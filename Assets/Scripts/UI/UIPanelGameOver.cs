using UnityEngine;
using UnityEngine.UI;

public class UIPanelGameOver : MonoBehaviour, IMenu
{
    [SerializeField] private Button btnClose;
    [SerializeField] private Text txtResult; // Result text displaying "YOU WIN!" or "GAME OVER!"

    private UIMainManager m_mngr;
    private GameManager m_gameManager;

    private void Awake()
    {
        if (btnClose) btnClose.onClick.AddListener(OnClickClose);
    }

    private void OnDestroy()
    {
        if (btnClose) btnClose.onClick.RemoveAllListeners();
    }

    private void OnClickClose()
    {
        m_mngr.ShowMainMenu();
    }

    public void Setup(UIMainManager mngr)
    {
        m_mngr = mngr;
        m_gameManager = FindObjectOfType<GameManager>();
    }

    /// <summary>
    /// Displays game over screen and updates result message.
    /// </summary>
    public void Show()
    {
        this.gameObject.SetActive(true);

        if (txtResult != null && m_gameManager != null)
        {
            txtResult.text = m_gameManager.IsGameWon ? "YOU WIN! 🎉" : "GAME OVER! 💀";
        }
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }
}
