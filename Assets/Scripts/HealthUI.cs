using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class HealthUI : MonoBehaviour
{
    public GameObject heartPrefab;
    public Transform heartContainer;

    private List<GameObject> hearts = new List<GameObject>();

    void OnEnable()
    {
        if (PlayerStats.Instance != null)
        {
            PlayerStats.Instance.OnHealthChanged += UpdateHearts;

            UpdateHearts(PlayerStats.Instance.currentHealth, PlayerStats.Instance.maxHealth);
        }
    }

    void OnDisable()
    {
        if (PlayerStats.Instance != null)
        {
            PlayerStats.Instance.OnHealthChanged -= UpdateHearts;
        }
    }

    void UpdateHearts(int current, int max)
    {
        if (heartContainer == null || heartPrefab == null) return;

        foreach (GameObject heart in hearts)
        {
            if (heart != null) Destroy(heart);
        }
        hearts.Clear();

        foreach (Transform child in heartContainer)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < max; i++)
        {
            GameObject newHeart = Instantiate(heartPrefab, heartContainer);
            hearts.Add(newHeart);
        }

        for (int i = 0; i < hearts.Count; i++)
        {
            Image heartImage = hearts[i].GetComponent<Image>();

            if (heartImage != null)
            {
                heartImage.enabled = i < current;
            }
        }
    }
}