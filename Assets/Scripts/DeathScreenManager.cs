using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScreenManager : MonoBehaviour
{
    public static DeathScreenManager Instance;
    public GameObject deathScreen;

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
    }

    public void ShowDeathScreen()
    {
        if (deathScreen != null)
        {
            deathScreen.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void RestartLevel()
    {
       
        if (deathScreen != null)
        {
            deathScreen.SetActive(false);
        }

       
        if (PlayerStats.Instance != null)
        {
            PlayerStats.Instance.ResetGame();
        }

       
        MainMenuManager.ShowMenuOnStart = false;
        Time.timeScale = 1f;

        
        SceneManager.LoadScene("Level1");
    }
}