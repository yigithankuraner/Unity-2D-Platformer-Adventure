using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogBehavior : MonoBehaviour
{
    public bool spriteFacesLeft = false;

    public Transform pointA;
    public Transform pointB;
    public float patrolSpeed = 2f;

    public float chaseSpeed = 6f;
    public float detectionRange = 5f;
    public float stopDistance = 1.5f;

    public float attackRate = 1.5f;
    public int damage = 1;

    private Transform player;
    private Transform currentPatrolTarget;
    private Animator anim;
    private bool isChasing = false;
    private float nextAttackTime = 0f;

    void Start()
    {
        anim = GetComponent<Animator>();
        currentPatrolTarget = pointB;

        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    void Update()
    {
        if (player == null) return;
        if (pointA == null || pointB == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer < detectionRange)
        {
            isChasing = true;
            ChaseAndAttack(distanceToPlayer);
        }
        else
        {
            isChasing = false;
            Patrol();
        }
    }

    void Patrol()
    {
        anim.SetBool("isMoving", true);
        anim.SetBool("isChasing", false);

        Vector2 targetPos = new Vector2(currentPatrolTarget.position.x, transform.position.y);
        transform.position = Vector2.MoveTowards(transform.position, targetPos, patrolSpeed * Time.deltaTime);

        FaceTarget(currentPatrolTarget.position.x);

        if (Vector2.Distance(transform.position, currentPatrolTarget.position) < 0.5f)
        {
            if (currentPatrolTarget == pointA)
                currentPatrolTarget = pointB;
            else
                currentPatrolTarget = pointA;
        }
    }

    void ChaseAndAttack(float distance)
    {
        FaceTarget(player.position.x);

        if (distance > stopDistance)
        {
            anim.SetBool("isMoving", true);
            anim.SetBool("isChasing", true);
            Vector2 targetPos = new Vector2(player.position.x, transform.position.y);
            transform.position = Vector2.MoveTowards(transform.position, targetPos, chaseSpeed * Time.deltaTime);
        }
        else
        {
            anim.SetBool("isMoving", false);
            anim.SetBool("isChasing", false);

            if (Time.time >= nextAttackTime)
            {
                anim.SetTrigger("attack");
                nextAttackTime = Time.time + attackRate;

                player.SendMessage("TakeDamage", damage, SendMessageOptions.DontRequireReceiver);
            }
        }
    }

    void FaceTarget(float targetX)
    {
        float direction = targetX - transform.position.x;
        if (Mathf.Abs(direction) < 0.1f) return;

        Vector3 scale = transform.localScale;

        if (direction > 0)
            scale.x = spriteFacesLeft ? -Mathf.Abs(scale.x) : Mathf.Abs(scale.x);
        else
            scale.x = spriteFacesLeft ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);

        transform.localScale = scale;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, stopDistance);

        if (pointA != null && pointB != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(pointA.position, pointB.position);
        }
    }
}