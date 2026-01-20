using UnityEngine;

public class DinoBehavior : MonoBehaviour
{
    public float walkSpeed = 2f;
    public float runSpeed = 5f;

    public Transform pointA;
    public Transform pointB;

    public float visionRange = 8f;
    public float attackRange = 1.5f;

    public float attackCooldown = 2f;
    public float attackDelay = 1f;

    public Transform player;

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer spriteRenderer;

    private float attackTimer;
    private float prepareTimer;
    private Transform currentTarget;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        currentTarget = pointB;

        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(new Vector2(transform.position.x, transform.position.y), new Vector2(player.position.x, player.position.y));

        attackTimer -= Time.deltaTime;

        if (distanceToPlayer < attackRange)
        {
            StopAndAttack();
        }
        else
        {
            prepareTimer = 0;

            if (distanceToPlayer < visionRange)
            {
                ChasePlayer();
            }
            else
            {
                Patrol();
            }
        }
    }
    [Header("Contact Damage")]
public int contactDamage = 1;

private void OnCollisionEnter2D(Collision2D collision)
{
    if (!collision.gameObject.CompareTag("Player")) return;

    PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
    if (playerHealth != null)
    {
        playerHealth.TakeDamage(contactDamage);
    }
}


    void Patrol()
    {
        if (anim != null) anim.SetBool("isRunning", false);

        transform.position = Vector2.MoveTowards(transform.position, currentTarget.position, walkSpeed * Time.deltaTime);

        float distanceToTarget = Vector2.Distance(new Vector2(transform.position.x, transform.position.y), new Vector2(currentTarget.position.x, currentTarget.position.y));

        if (distanceToTarget < 0.2f)
        {
            if (currentTarget == pointB) currentTarget = pointA;
            else currentTarget = pointB;
        }

        if (currentTarget.position.x > transform.position.x) spriteRenderer.flipX = true;
        else spriteRenderer.flipX = false;
    }

    void ChasePlayer()
    {
        if (anim != null) anim.SetBool("isRunning", true);

        if (player.position.x > transform.position.x)
        {
            spriteRenderer.flipX = true;
            rb.linearVelocity = new Vector2(runSpeed, rb.linearVelocity.y);
        }
        else
        {
            spriteRenderer.flipX = false;
            rb.linearVelocity = new Vector2(-runSpeed, rb.linearVelocity.y);
        }
    }

    void StopAndAttack()
    {
        rb.linearVelocity = Vector2.zero;

        if (anim != null) anim.SetBool("isRunning", false);

        if (player.position.x > transform.position.x) spriteRenderer.flipX = true;
        else spriteRenderer.flipX = false;

        prepareTimer += Time.deltaTime;

        if (prepareTimer >= attackDelay && attackTimer <= 0)
        {
            if (anim != null) anim.SetTrigger("attack");
            attackTimer = attackCooldown;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, visionRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        if (pointA != null && pointB != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(pointA.position, pointB.position);
        }
    }
}