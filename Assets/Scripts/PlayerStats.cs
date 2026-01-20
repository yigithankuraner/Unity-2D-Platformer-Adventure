using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance;

    [Header("Başlangıç Ayarları")]
    public int defaultMaxHealth = 3;
    public float defaultMoveSpeed = 5f;
    public int defaultDamage = 1; 

    [Header("Anlık Durum")]
    public int maxHealth;
    public int currentHealth;
    public float moveSpeed;
    public int damage; 
    public int gold = 0;

    public event Action<int, int> OnHealthChanged; 
    public event Action<int> OnGoldChanged;        

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            maxHealth = defaultMaxHealth;
            currentHealth = maxHealth;
            moveSpeed = defaultMoveSpeed;
            damage = defaultDamage;
            gold = 0;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Invoke("ForceUIUpdate", 0.1f);
    }

    void ForceUIUpdate()
    {
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        OnGoldChanged?.Invoke(gold);
    }

    public void ResetGame()
    {
        maxHealth = defaultMaxHealth;
        currentHealth = maxHealth;
        damage = defaultDamage; 
        moveSpeed = defaultMoveSpeed;
        gold = 0;

        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        OnGoldChanged?.Invoke(gold);
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            if (DeathScreenManager.Instance != null)
            {
                DeathScreenManager.Instance.ShowDeathScreen();
            }
        }

        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public void IncreaseMaxHealth(int amount)
    {
        maxHealth += amount;
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public void AddGold(int amount)
    {
        gold += amount;
        OnGoldChanged?.Invoke(gold);
    }

    public bool SpendGold(int amount)
    {
        if (gold < amount)
        {
            return false;
        }

        gold -= amount;
        OnGoldChanged?.Invoke(gold);
        return true;
    }
}