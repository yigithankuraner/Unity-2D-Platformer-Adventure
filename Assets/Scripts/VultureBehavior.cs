using UnityEngine;

public class VultureBehavior : MonoBehaviour
{
    public float flySpeed = 4f;
    public float detectRange = 10f;
    public float yTolerance = 1.5f;
    public Transform player;

    private SpriteRenderer spriteRenderer;
    private Animator anim;
    private bool isFlying = false;
    private Vector3 startPosition;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        startPosition = transform.position;

        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        float heightDifference = Mathf.Abs(transform.position.y - player.position.y);

        if (!isFlying)
        {
            if (distanceToPlayer < detectRange && heightDifference < yTolerance)
            {
                isFlying = true;
                if (anim != null) anim.SetBool("isFlying", true);
            }
        }
        else
        {
            ChasePlayer();
        }
    }
    public int damage = 1;

private void OnTriggerEnter2D(Collider2D other)
{
    if (!other.CompareTag("Player")) return;

    PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
    if (playerHealth != null)
    {
        playerHealth.TakeDamage(damage);
    }
}


    void ChasePlayer()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.position, flySpeed * Time.deltaTime);

        if (player.position.x > transform.position.x)
        {
            spriteRenderer.flipX = false;
        }
        else
        {
            spriteRenderer.flipX = true;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectRange);
    }
}