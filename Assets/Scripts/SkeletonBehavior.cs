using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBehavior : MonoBehaviour
{
    public float detectionRange = 5f;
    public float moveSpeed = 2f;
    public float reviveTime = 4f;
    public int healthPhase1 = 2;
    public int healthPhase2 = 4;

    public int goldReward = 6;
    public GameObject goldPopupPrefab;
    public float popupYOffset = 0.3f;
    public GameObject deathEffectPrefab;

    public Transform player;
    public BoxCollider2D bodyCollider;

    public int contactDamage = 1;

    private Animator anim;
    private Rigidbody2D rb;
    private bool isDead = true;
    private bool isRising = false;
    private bool isSecondPhase = false;
    private int currentHealth;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        if (player == null && GameObject.FindGameObjectWithTag("Player") != null)
            player = GameObject.FindGameObjectWithTag("Player").transform;

        currentHealth = healthPhase1;
        bodyCollider.enabled = false;
        rb.bodyType = RigidbodyType2D.Static;
        isDead = true;
    }

    void Update()
    {
        if (this == null) return;

        if (isDead)
        {
            HandleDormantState();
        }
        else if (!isRising)
        {
            HandleActiveState();
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead || isRising) return;

        currentHealth -= damage;
        anim.SetTrigger("hit");

        if (currentHealth <= 0)
        {
            if (isSecondPhase)
            {
                DiePermanently();
            }
            else
            {
                StartCoroutine(DieAndPrepareNextPhase());
            }
        }
    }

    IEnumerator DieAndPrepareNextPhase()
    {
        isRising = true;
        isDead = true;

        bodyCollider.enabled = false;
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Static;

        anim.SetTrigger("die");

        isSecondPhase = true;

        yield return new WaitForSeconds(reviveTime);

        isRising = false;
    }

    void DiePermanently()
    {
        if (PlayerStats.Instance != null)
        {
            PlayerStats.Instance.AddGold(goldReward);
        }

        if (deathEffectPrefab != null)
        {
            Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
        }

        if (goldPopupPrefab != null)
        {
            Vector3 popupPosition = transform.position + new Vector3(0, popupYOffset, 0);
            Instantiate(goldPopupPrefab, popupPosition, Quaternion.identity);
        }

        anim.SetTrigger("die");
        bodyCollider.enabled = false;
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Static;

        this.enabled = false;
        Destroy(gameObject, 0.5f);
    }

    void HandleDormantState()
    {
        if (player == null) return;
        float distance = Vector2.Distance(transform.position, player.position);

        if (distance < detectionRange && !isRising)
        {
            StartCoroutine(ReviveRoutine());
        }
    }

    IEnumerator ReviveRoutine()
    {
        isRising = true;
        anim.SetTrigger("revive");

        yield return new WaitForSeconds(1f);

        rb.bodyType = RigidbodyType2D.Dynamic;
        isDead = false;
        isRising = false;
        bodyCollider.enabled = true;

        currentHealth = healthPhase2;
    }

    void HandleActiveState()
    {
        if (player == null) return;

        float direction = Mathf.Sign(player.position.x - transform.position.x);
        rb.linearVelocity = new Vector2(direction * moveSpeed, rb.linearVelocity.y);

        if ((direction > 0 && transform.localScale.x > 0) || (direction < 0 && transform.localScale.x < 0))
        {
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead || isRising) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            if (PlayerStats.Instance != null)
            {
                PlayerStats.Instance.TakeDamage(contactDamage);
            }
        }
    }
}