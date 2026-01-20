using UnityEngine;
using TMPro;

public class GoldManager : MonoBehaviour
{
    public TMP_Text goldText;

    void Start()
    {
        if (PlayerStats.Instance != null)
        {
            PlayerStats.Instance.OnGoldChanged += UpdateUI;
            UpdateUI(PlayerStats.Instance.gold);
        }
    }

    void OnDestroy()
    {
        if (PlayerStats.Instance != null)
        {
            PlayerStats.Instance.OnGoldChanged -= UpdateUI;
        }
    }

    void UpdateUI(int gold)
    {
        goldText.text = gold.ToString();
    }
}
