using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public GameObject mainMenuPanel;

    public static bool ShowMenuOnStart = true;

    void Awake()
    {
        if (mainMenuPanel == null) return;

        if (ShowMenuOnStart)
        {
            mainMenuPanel.SetActive(true);
            Time.timeScale = 0f;
        }
        else
        {
            mainMenuPanel.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    public void NewGame()
    {
        ShowMenuOnStart = false; 
        if (mainMenuPanel != null)
        {
            mainMenuPanel.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    public void BackToMainMenu()
    {
        ShowMenuOnStart = true; 
        Time.timeScale = 1f;
        SceneManager.LoadScene("Level1");
    }
}