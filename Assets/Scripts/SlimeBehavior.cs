using UnityEngine;

public class SlimeBehavior : MonoBehaviour
{
    public float jumpForceY = 500f;
    public float jumpForceX = 300f;
    public float jumpDelay = 1.5f;

    public float yTolerance = 1f;
    public float detectRange = 15f;

    public Transform player;
    public Transform groundCheckPoint;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer spriteRenderer;

    private float jumpTimer;
    private bool isGrounded;
    private bool isChasing = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        jumpTimer = jumpDelay;

        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }
    }

    void Update()
    {
        if (player == null) return;

        isGrounded = Physics2D.OverlapCircle(groundCheckPoint.position, 0.2f, groundLayer);

        if (anim != null) anim.SetBool("isGrounded", isGrounded);

        if (!isGrounded) return;

        jumpTimer -= Time.deltaTime;

        float heightDifference = Mathf.Abs(transform.position.y - player.position.y);
        float horizontalDistance = Vector2.Distance(new Vector2(transform.position.x, 0), new Vector2(player.position.x, 0));

        if (heightDifference < yTolerance && horizontalDistance < detectRange)
        {
            isChasing = true;
        }
        else
        {
            isChasing = false;
        }

        if (isChasing && jumpTimer <= 0)
        {
            JumpTowardsPlayer();
            jumpTimer = jumpDelay;
        }
    }
    public int damage = 1;

private void OnCollisionEnter2D(Collision2D collision)
{
    if (!collision.gameObject.CompareTag("Player")) return;

    PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
    if (playerHealth != null)
    {
        playerHealth.TakeDamage(damage);
    }
}


    void JumpTowardsPlayer()
    {
        if (anim != null) anim.SetTrigger("jump");

        float direction = (player.position.x > transform.position.x) ? 1f : -1f;

        spriteRenderer.flipX = (direction > 0);

        rb.AddForce(new Vector2(jumpForceX * direction, jumpForceY), ForceMode2D.Impulse);
    }

    void OnDrawGizmos()
    {
        if (groundCheckPoint != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(groundCheckPoint.position, 0.2f);
        }
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectRange);
    }
}