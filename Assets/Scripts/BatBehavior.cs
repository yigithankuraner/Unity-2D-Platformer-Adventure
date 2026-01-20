using UnityEngine;
using System.Collections;

public class BatBehavior : MonoBehaviour
{
    
    public float flySpeed = 4f;
    public float detectionRange = 7f;
    public float zigzagAmount = 2f;
    public float zigzagSpeed = 5f;
    public int damage = 1;

    private Transform player;
    private Animator anim;
    private bool isFlying = false;
    private bool isRetreating = false;

    void Start()
    {
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

        if (!isFlying && distance < detectionRange)
        {
            isFlying = true;
            if (anim != null) anim.SetBool("isFlying", true);
        }

        if (isFlying)
        {
            FlyTowardsPlayer();
        }
    }

    void FlyTowardsPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        Vector2 perpendicular = new Vector2(-direction.y, direction.x);
        float offset = Mathf.Sin(Time.time * zigzagSpeed) * zigzagAmount;

        Vector2 targetVelocity = (direction * flySpeed) + (perpendicular * offset);
        transform.Translate(targetVelocity * Time.deltaTime);

        FlipSprite(player.position.x);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isRetreating)
        {
            collision.SendMessage("TakeDamage", damage, SendMessageOptions.DontRequireReceiver);
            StartCoroutine(RetreatRoutine());
        }
    }

    IEnumerator RetreatRoutine()
    {
        isRetreating = true;
        Vector2 retreatDirection = (transform.position - player.position).normalized;
        float retreatEndTime = Time.time + 2f;

        while (Time.time < retreatEndTime)
        {
            transform.Translate(retreatDirection * (flySpeed * 0.8f) * Time.deltaTime);
            yield return null;
        }

        isRetreating = false;
    }

    void FlipSprite(float targetX)
    {
        if (targetX > transform.position.x)
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, 1);
        else
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, 1);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}