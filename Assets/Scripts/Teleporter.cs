using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public Transform destination;
    private static float lastTeleportTime;
    private float teleportCooldown = 2f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (Time.time < lastTeleportTime + teleportCooldown) return;

        if (other.CompareTag("Player"))
        {
            lastTeleportTime = Time.time;
            other.transform.position = destination.position;

            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
            }
        }
    }
}