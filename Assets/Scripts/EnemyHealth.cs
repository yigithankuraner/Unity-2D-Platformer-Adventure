using UnityEngine;
using TMPro;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 2;
    private int currentHealth;

    public int goldReward = 5;
    public GameObject goldPopupPrefab;
    public float popupYOffset = 0.3f;
    public GameObject deathEffectPrefab; 

    private SpriteRenderer sr;
    private Color originalColor;

    void Start()
    {
        currentHealth = maxHealth;
        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;
    }

    public void TakeDamage(int damage)
    {

        currentHealth -= damage;
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.Play(SoundManager.Instance.enemyHit);
        }
        StopAllCoroutines();
        StartCoroutine(HitFlash());

        if (currentHealth <= 0)
            Die();
    }

    IEnumerator HitFlash()
    {
        sr.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sr.color = originalColor;
    }

    void Die()
{
        if (deathEffectPrefab != null)
        {
            Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
        }
        if (PlayerStats.Instance != null)
        PlayerStats.Instance.AddGold(goldReward);

    GameObject popup = Instantiate(
        goldPopupPrefab,
        transform.position,
        Quaternion.identity
    );

    popup.transform.SetParent(transform);
    popup.transform.localPosition = new Vector3(0f, popupYOffset, 0f);
    popup.transform.SetParent(null);

    TMP_Text t = popup.GetComponent<TMP_Text>();
    if (t != null)
        t.text = "+" + goldReward;


    Destroy(gameObject);
}
}
