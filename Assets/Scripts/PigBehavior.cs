using UnityEngine;
using System.Collections;

public class PigBehavior : MonoBehaviour
{

    public bool spriteFacesLeft = true;

    
    public float runSpeed = 5f;
    public float detectionRange = 6f;
    public int damage = 1;
    public float knockbackForce = 10f;

    private Transform player;
    private Animator anim;
    private bool isChasing = false;
    private bool isRetreating = false;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    void Update()
    {
        if (player == null || isRetreating) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (!isChasing && distance < detectionRange)
        {
            isChasing = true;
            if (anim != null) anim.SetBool("isRunning", true);
        }

        if (isChasing)
        {
            MoveTowardsPlayer();
        }
    }

    void MoveTowardsPlayer()
    {
        float directionX = player.position.x > transform.position.x ? 1 : -1;
        rb.linearVelocity = new Vector2(directionX * runSpeed, rb.linearVelocity.y);
        FlipSprite(directionX);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isRetreating)
        {
            ApplyKnockback(collision.gameObject);
            collision.gameObject.SendMessage("TakeDamage", damage, SendMessageOptions.DontRequireReceiver);
            StartCoroutine(RetreatRoutine());
        }
    }

    void ApplyKnockback(GameObject playerObj)
    {
        Rigidbody2D playerRb = playerObj.GetComponent<Rigidbody2D>();
        if (playerRb != null)
        {
            Vector2 knockbackDir = (playerObj.transform.position - transform.position).normalized;
            playerRb.linearVelocity = Vector2.zero;
            playerRb.AddForce(new Vector2(knockbackDir.x, 0.5f) * knockbackForce, ForceMode2D.Impulse);
        }
    }

    IEnumerator RetreatRoutine()
    {
        isRetreating = true;
        isChasing = false;
        if (anim != null) anim.SetBool("isRunning", false);

        float retreatDir = player.position.x > transform.position.x ? -1 : 1;
        rb.linearVelocity = new Vector2(retreatDir * runSpeed * 0.5f, rb.linearVelocity.y);
        FlipSprite(retreatDir);

        yield return new WaitForSeconds(2f);

        rb.linearVelocity = Vector2.zero;
        isRetreating = false;
    }

    void FlipSprite(float moveDir)
    {
        Vector3 currentScale = transform.localScale;
        float baseScaleX = Mathf.Abs(currentScale.x);

        if (moveDir > 0)
        {
            currentScale.x = spriteFacesLeft ? -baseScaleX : baseScaleX;
        }
        else if (moveDir < 0)
        {
            currentScale.x = spriteFacesLeft ? baseScaleX : -baseScaleX;
        }

        transform.localScale = currentScale;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}