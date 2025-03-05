using TMPro;
using UnityEngine;

public class UI_Manager : MonoBehaviour
{
    public static UI_Manager instance;

    [Header("Canvas")]
    [SerializeField] Canvas gameCanvas;

    [Header("UI Events")]
    [SerializeField] GameObject escapeUI;
    [SerializeField] GameObject lostUI;
    [SerializeField] GameObject winUI;

    [Header("Text References")]
    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] TextMeshProUGUI goldText;
    [SerializeField] TextMeshProUGUI statisticsText;
    [SerializeField] TextMeshProUGUI statisticsDescriptionText;

    void OnEnable()
    {
        GameManager.instance.OnLost += ShowLostUI;
        GameManager.instance.OnWin += ShowWinUI;
        GameManager.instance.OnHealthChanged += UpdateHealthText;
        ShopManager.instance.OnGoldChanged += UpdateGoldText;
    }

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        Time.timeScale = 1;

        gameCanvas.gameObject.SetActive(true);
        escapeUI.SetActive(false);
        lostUI.SetActive(false);
        winUI.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            EscapeSwitch();
        }
    }

    public void ShowLostUI()
    {
        gameCanvas.gameObject.SetActive(false);
        escapeUI.SetActive(false);
        winUI.SetActive(false);

        lostUI.SetActive(true);
        Time.timeScale = 0;
    }

    public void ShowWinUI()
    {
        gameCanvas.gameObject.SetActive(false);
        escapeUI.SetActive(false);
        lostUI.SetActive(false);

        winUI.SetActive(true);
        Time.timeScale = 0;
    }

    public void EscapeSwitch()
    {
        if (winUI.activeSelf || lostUI.activeSelf)
        {
            return;
        }

        if (escapeUI.activeSelf)
        {
            escapeUI.SetActive(false);
            gameCanvas.gameObject.SetActive(true);
            Time.timeScale = 1;

            return;
        }

        gameCanvas.gameObject.SetActive(false);
        escapeUI.SetActive(true);
        Time.timeScale = 0;
    }

    void UpdateHealthText(int health)
    {
        healthText.text = "Health: " + health + "/3"; // 3 is always the max health
    }

    void UpdateGoldText(int gold)
    {
        goldText.text = "Gold: " + gold;
    }

    public void SetStatistics(string name, string description, AttackType type, int health, int damage)
    {
        statisticsText.text = "Name: " + name + "\n" +
                        "Type: " + type + "\n" +
                        "Health: " + health + "\n" +
                        "Damage: " + damage;

        statisticsDescriptionText.text = description;

    }

    void OnDisable()
    {
        GameManager.instance.OnLost -= ShowLostUI;
        GameManager.instance.OnWin -= ShowWinUI;
        GameManager.instance.OnHealthChanged -= UpdateHealthText;
        ShopManager.instance.OnGoldChanged -= UpdateGoldText;
    }
}
