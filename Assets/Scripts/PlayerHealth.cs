using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public float invincibilityDuration = 0.2f;
    private float nextDamageTime;

    private SpriteRenderer sr;
    private Color originalColor;

    void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        if (sr != null)
            originalColor = sr.color;
    }

    public void TakeDamage(int damage)
    {

        if (Time.time < nextDamageTime) return;
        nextDamageTime = Time.time + invincibilityDuration;

        if (PlayerStats.Instance == null) return;

        PlayerStats.Instance.TakeDamage(damage);
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.Play(SoundManager.Instance.playerHit);
        }


        StopAllCoroutines();
        StartCoroutine(HitFlash());

        if (PlayerStats.Instance.currentHealth <= 0)
            Die();
    }

    IEnumerator HitFlash()
    {
        if (sr == null) yield break;

        sr.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sr.color = originalColor;
    }

   void Die()
{
    Debug.Log("PLAYER DEAD");

    PlayerMovement movement = GetComponent<PlayerMovement>();
    if (movement != null)
        movement.enabled = false;

    Weapon weapon = GetComponent<Weapon>();
    if (weapon != null)
        weapon.enabled = false;

    Rigidbody2D rb = GetComponent<Rigidbody2D>();
    if (rb != null)
    {
        rb.linearVelocity = Vector2.zero;
        rb.simulated = false;
    }

    Collider2D[] colliders = GetComponentsInChildren<Collider2D>();
    foreach (Collider2D col in colliders)
        col.enabled = false;

    if (DeathScreenManager.Instance != null)
    {
        DeathScreenManager.Instance.ShowDeathScreen();
    }
    else
    {
        Debug.LogError("DeathScreenManager.Instance NULL! Sahneye ekli mi?");
    }
}

}
