using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public float lifeTime = 2f;
    public int damage = 1;
    public string ownerTag;

    private bool hasHit = false;

    void Start() { Destroy(gameObject, lifeTime); }

    void Update() { transform.Translate(Vector2.right * speed * Time.deltaTime); }

    void OnTriggerEnter2D(Collider2D other)
    {
       
        if (hasHit || string.IsNullOrEmpty(ownerTag) || other.CompareTag(ownerTag))
            return;

       
        if (other.CompareTag("Player"))
        {
            
            PlayerHealth health = other.GetComponent<PlayerHealth>();
            if (health != null)
            {
                hasHit = true;
                health.TakeDamage(damage);
            }
           
            else
            {
                PlayerStats stats = PlayerStats.Instance; 
                if (stats == null) stats = other.GetComponent<PlayerStats>();

                if (stats != null)
                {
                    hasHit = true;
                    stats.TakeDamage(damage);
                }
            }
        }
        
        else if (other.CompareTag("Enemy"))
        {
            bool hitSomething = false;

           
            EnemyHealth normalEnemy = other.GetComponent<EnemyHealth>();
            if (normalEnemy != null)
            {
                normalEnemy.TakeDamage(damage);
                hitSomething = true;
            }

           
            SkeletonBehavior skeletonBoss = other.GetComponent<SkeletonBehavior>();
            if (skeletonBoss != null)
            {
                skeletonBoss.TakeDamage(damage);
                hitSomething = true;
            }

            if (hitSomething) hasHit = true;
        }

        
        Destroy(gameObject);
    }
}