using UnityEngine;

public class FrogBehavior : MonoBehaviour
{
    public float jumpForceX = 300f;
    public float jumpForceY = 500f;
    public float jumpDelay = 2f;
    public LayerMask groundLayer;

    public float attackRange = 7f;
    public float yTolerance = 2f;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 2f;

    public Transform pointA;
    public Transform pointB;
    public Transform player;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    private float jumpTimer;
    private float fireTimer;

    private Transform currentTarget;
    private bool facingRight = true;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }

        currentTarget = pointB;
        fireTimer = fireRate;

        if (spriteRenderer != null)
            spriteRenderer.flipX = facingRight;
    }

    void Update()
    {
        if (player == null) return;

        fireTimer -= Time.deltaTime;
        jumpTimer -= Time.deltaTime;

        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 1.2f, groundLayer);

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        float heightDifference = Mathf.Abs(transform.position.y - player.position.y);

        if (distanceToPlayer <= attackRange && heightDifference <= yTolerance)
        {
            AttackBehavior();
        }
        else
        {
            PatrolBehavior();
        }
    }

    void PatrolBehavior()
    {
        LookAtTarget(currentTarget.position.x);

        if (jumpTimer <= 0 && isGrounded)
        {
            Jump();
            jumpTimer = jumpDelay;
        }

        if (Mathf.Abs(transform.position.x - currentTarget.position.x) < 2f && isGrounded)
        {
            currentTarget = (currentTarget == pointB) ? pointA : pointB;
        }
    }

    void AttackBehavior()
    {
        LookAtTarget(player.position.x);

        if (fireTimer <= 0f)
        {
            Shoot();
            fireTimer = fireRate;
        }
    }

    void Jump()
    {
        float direction = facingRight ? 1 : -1;

        rb.linearVelocity = Vector2.zero;
        rb.AddForce(new Vector2(jumpForceX * direction, jumpForceY), ForceMode2D.Impulse);
    }

    void Shoot()
    {
        if (bulletPrefab != null && firePoint != null)
        {
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        }
    }

    void LookAtTarget(float targetX)
    {
        if (targetX > transform.position.x && !facingRight) Flip();
        else if (targetX < transform.position.x && facingRight) Flip();
    }

    void Flip()
    {
        facingRight = !facingRight;

        if (spriteRenderer != null)
            spriteRenderer.flipX = facingRight;

        if (firePoint != null)
            firePoint.Rotate(0f, 180f, 0f);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * 1.2f);
    }
}
