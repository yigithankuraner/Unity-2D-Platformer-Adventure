using UnityEngine;

public class ChestWin : MonoBehaviour
{
    private bool isPlayerNearby = false;

    void Update()
    {
     
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            if (WinScreenManager.Instance != null)
            {
                WinScreenManager.Instance.ShowWinScreen(); 
            }
            else
            {
                Debug.LogError("WinScreenManager sahnede bulunamadý!");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) 
        {
            isPlayerNearby = true;
            Debug.Log("Sandýða yaklaþtýn, 'E' tuþuna bas!");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
        }
    }
}