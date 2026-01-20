using UnityEngine;

public class GoldPickup : MonoBehaviour
{
    public int goldAmount = 1;

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (!other.CompareTag("Player")) return;

        if (PlayerStats.Instance != null)
        {
            PlayerStats.Instance.AddGold(goldAmount);
        }

        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.Play(SoundManager.Instance.gold);
        }

        Destroy(gameObject);
    }
}