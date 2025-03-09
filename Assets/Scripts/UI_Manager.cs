using TMPro;
using UnityEngine;

public class UI_Manager : MonoBehaviour
{
    public static UI_Manager instance;

    [Header("Canvas")]
    [SerializeField] Canvas gameCanvas;

    [Header("Events UI")]
    [SerializeField] GameObject managerUI;
    [SerializeField] TextMeshProUGUI coveringText;

    [Header("Text References")]
    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] TextMeshProUGUI goldText;
    [SerializeField] TextMeshProUGUI statisticsText;
    [SerializeField] TextMeshProUGUI statisticsDescriptionText;

    bool isLost;

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
        isLost = false;

        gameCanvas.gameObject.SetActive(true);
        managerUI.SetActive(false);
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
        isLost = true;
        gameCanvas.gameObject.SetActive(false);
        
        managerUI.SetActive(true);
        coveringText.text = "You have Lost!";

        Time.timeScale = 0;
    }

    public void ShowWinUI()
    {
        if (isLost)
        {
            return;
        }

        gameCanvas.gameObject.SetActive(false);
        
        managerUI.SetActive(true);
        coveringText.text = "You have Won!";

        Time.timeScale = 0;
    }

    public void EscapeSwitch()
    {
        if (managerUI.activeSelf)
        {
            gameCanvas.gameObject.SetActive(true);
            managerUI.SetActive(false);

            Time.timeScale = 1;

            return;
        }

        gameCanvas.gameObject.SetActive(false);
        managerUI.SetActive(true);

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
