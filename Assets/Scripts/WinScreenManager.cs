using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScreenManager : MonoBehaviour
{
    public static WinScreenManager Instance;
    public GameObject winScreen;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        if (winScreen != null)
        {
            winScreen.SetActive(false);
        }
    }

    public void ShowWinScreen()
    {
        if (winScreen != null)
        {
            winScreen.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void RestartGame()
    {
        MainMenuManager.ShowMenuOnStart = false;

        if (winScreen != null)
            winScreen.SetActive(false);
        if (PlayerStats.Instance != null)
        {
            PlayerStats.Instance.ResetGame();
        }
        Time.timeScale = 1f;
        SceneManager.LoadScene("Level1");
    }
}