using UnityEngine;

public class UI_Manager : MonoBehaviour
{
    [SerializeField] Canvas gameCanvas;

    [SerializeField] GameObject escapeUI;
    [SerializeField] GameObject lostUI;
    [SerializeField] GameObject winUI;

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
}
