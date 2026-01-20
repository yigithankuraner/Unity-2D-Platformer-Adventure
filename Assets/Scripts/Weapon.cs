using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 0.3f;

    private float nextFireTime;
    private SpriteRenderer playerSR;
    private Vector3 firePointStartPos;

    void Start()
    {
        playerSR = GetComponentInParent<SpriteRenderer>();

        firePointStartPos = firePoint.localPosition;
    }

    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }

        UpdateFirePointDirection();
    }

    void UpdateFirePointDirection()
    {
        int dir = playerSR.flipX ? -1 : 1;

        firePoint.localPosition = new Vector3(
            Mathf.Abs(firePointStartPos.x) * dir,
            firePointStartPos.y,
            firePointStartPos.z
        );
    }

    void Shoot()
    {

        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.Play(SoundManager.Instance.shoot);
        }
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        Bullet b = bullet.GetComponent<Bullet>();
        b.ownerTag = "Player";
        b.damage = PlayerStats.Instance.damage;

        if (playerSR.flipX)
            bullet.transform.rotation = Quaternion.Euler(0, 0, 180);
    }
}
