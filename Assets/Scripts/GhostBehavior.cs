using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostBehavior : MonoBehaviour
{
    [Header("Settings")]
    public float moveSpeed = 3f;
    public float detectionRange = 10f;
    public float stopDistance = 1.5f;
    public float verticalTolerance = 2.5f;

    [Header("Wave Settings")]
    public float waveFrequency = 5f;
    public float waveAmplitude = 1f;

    private Transform player;
    private SpriteRenderer spriteRenderer;
    private Animator anim;
    private float startY;

    void Start()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        startY = transform.position.y;

        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        float heightDifference = Mathf.Abs(player.position.y - transform.position.y);

        if (distanceToPlayer < detectionRange && heightDifference < verticalTolerance)
        {
            float newY = player.position.y + Mathf.Sin(Time.time * waveFrequency) * waveAmplitude;

            if (distanceToPlayer > stopDistance)
            {
                anim.SetBool("isWalking", true);

                float targetX = Mathf.MoveTowards(transform.position.x, player.position.x, moveSpeed * Time.deltaTime);
                transform.position = new Vector3(targetX, newY, 0);

                if (player.position.x > transform.position.x)
                    spriteRenderer.flipX = false;
                else
                    spriteRenderer.flipX = true;
            }
            else
            {
                anim.SetBool("isWalking", false);
                transform.position = new Vector3(transform.position.x, newY, 0);
            }
        }
        else
        {
            anim.SetBool("isWalking", false);
            float idleY = transform.position.y + (Mathf.Sin(Time.time * 2f) * 0.005f);
            transform.position = new Vector3(transform.position.x, idleY, 0);
        }
    }
    [Header("Damage")]
public int damage = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (PlayerStats.Instance != null)
        {
            PlayerStats.Instance.TakeDamage(damage);
        }
        else
        {
            PlayerStats stats = other.GetComponent<PlayerStats>();
            if (stats != null)
            {
                stats.TakeDamage(damage);
            }
        }
    }


    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, new Vector3(detectionRange * 2, verticalTolerance * 2, 0));
    }
}