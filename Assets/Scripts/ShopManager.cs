using UnityEngine;
using TMPro;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance;

    public GameObject shopPanel;
    public GameObject hudPanel;

    public TMP_Text shopGoldText;

    public int healthPrice = 10;
    public int damagePrice = 15;
    public int speedPrice = 12;

    public bool IsShopOpen { get; private set; } = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        if (shopPanel != null)
            shopPanel.SetActive(false);

        if (PlayerStats.Instance != null)
        {
            PlayerStats.Instance.OnGoldChanged += UpdateGoldUI;
            UpdateGoldUI(PlayerStats.Instance.gold);
        }
    }

    void OnDestroy()
    {
        if (PlayerStats.Instance != null)
            PlayerStats.Instance.OnGoldChanged -= UpdateGoldUI;
    }

    void UpdateGoldUI(int gold)
    {
        if (shopGoldText != null)
            shopGoldText.text = gold.ToString();
    }


    public void OpenShop()
    {
        if (IsShopOpen) return;

        IsShopOpen = true;
        shopPanel.SetActive(true);
        shopPanel.transform.SetAsLastSibling();
        hudPanel.SetActive(false);
        Time.timeScale = 0f;
    }

    public void CloseShop()
    {
        IsShopOpen = false;
        shopPanel.SetActive(false);
        hudPanel.SetActive(true);
        Time.timeScale = 1f;
    }


    public void BuyHealth()
    {
        if (!PlayerStats.Instance.SpendGold(healthPrice))
            return;

        PlayerStats.Instance.IncreaseMaxHealth(1);
    }

    public void BuyDamage()
    {
        if (!PlayerStats.Instance.SpendGold(damagePrice))
            return;

        PlayerStats.Instance.damage += 1;
    }

    public void BuySpeed()
    {
        if (!PlayerStats.Instance.SpendGold(speedPrice))
            return;

        PlayerStats.Instance.moveSpeed += 0.5f;
    }
}
