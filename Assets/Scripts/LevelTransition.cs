using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransition : MonoBehaviour
{
    public int sceneIndex;

    private bool playerIsAtDoor = false;
    private bool shopUsed = false;

    void Update()
    {
        if (ShopManager.Instance != null && ShopManager.Instance.IsShopOpen)
            return;

        if (!playerIsAtDoor) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!shopUsed)
            {
                if (ShopManager.Instance != null)
                {
                    ShopManager.Instance.OpenShop();
                    shopUsed = true;
                }
            }
            else
            {
                SceneManager.LoadScene(sceneIndex);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerIsAtDoor = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerIsAtDoor = false;
    }
}
