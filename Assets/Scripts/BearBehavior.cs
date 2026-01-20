using UnityEngine;

public class BearBehavior : MonoBehaviour
{
    public bool spriteFacesLeft = true;
    public Transform pointA;
    public Transform pointB;
    public float walkSpeed = 2f;
    public float chaseSpeed = 5f;
    public float detectionRange = 5f;
    public float knockbackForce = 12f;
    public int damage = 1;

    private Transform targetPoint;
    private Transform player;
    private Rigidbody2D rb;
    private Animator anim;
    private bool isChasing = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        targetPoint = pointB;

        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    void Update()
    {
        if (player != null && Vector2.Distance(transform.position, player.position) < detectionRange)
        {
            isChasing = true;
        }
        else
        {
            isChasing = false;
        }

        Move();
    }

    void Move()
    {
        float currentSpeed = isChasing ? chaseSpeed : walkSpeed;
        Vector2 direction;

        if (isChasing)
        {
            float diffX = player.position.x - transform.position.x;

            if (Mathf.Abs(diffX) < 0.1f)
            {
                rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
                if (anim != null) anim.SetBool("isWalking", false);
                return;
            }

            direction = new Vector2(Mathf.Sign(diffX), 0);
        }
        else
        {
            float distToTarget = targetPoint.position.x - transform.position.x;

            if (Mathf.Abs(distToTarget) < 0.2f)
            {
                targetPoint = (targetPoint == pointA) ? pointB : pointA;
                rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
                return;
            }

            direction = new Vector2(Mathf.Sign(distToTarget), 0);
        }

        rb.linearVelocity = new Vector2(direction.x * currentSpeed, rb.linearVelocity.y);

        if (anim != null) anim.SetBool("isWalking", true);

        FlipSprite(direction.x);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.SendMessage("TakeDamage", damage, SendMessageOptions.DontRequireReceiver);

            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                Vector2 dir = (collision.transform.position - transform.position).normalized;
                playerRb.linearVelocity = Vector2.zero;
                playerRb.AddForce(new Vector2(dir.x, 0.3f) * knockbackForce, ForceMode2D.Impulse);
            }
        }
    }

    void FlipSprite(float moveDirX)
    {
        if (Mathf.Abs(moveDirX) < 0.01f) return;

        Vector3 scale = transform.localScale;
        float baseScale = Mathf.Abs(scale.x);

        if (moveDirX > 0)
        {
            scale.x = spriteFacesLeft ? -baseScale : baseScale;
        }
        else if (moveDirX < 0)
        {
            scale.x = spriteFacesLeft ? baseScale : -baseScale;
        }

        transform.localScale = scale;
    }

    private void OnDrawGizmos()
    {
        if (pointA != null && pointB != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(pointA.position, pointB.position);
            Gizmos.DrawWireSphere(transform.position, detectionRange);
        }
    }
}