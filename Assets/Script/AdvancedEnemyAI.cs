using System.Collections;
using UnityEngine;

public class AdvancedEnemyAI : MonoBehaviour
{
    public Transform player;
    public GameObject bulletPrefab;
    public Transform shootingPoint;
    public float shootingRange = 15f;
    public float bulletSpeed = 30f;
    public float shootingCooldown = 1.5f;
    private float shootingTimer;

    public int maxAmmo = 5;
    private int currentAmmo;
    public float reloadTime = 2f;
    private bool isReloading = false;

    void Start()
    {
        currentAmmo = maxAmmo;
    }

    void Update()
    {
        if (isReloading)
            return;

        if (currentAmmo <= 0)
        {
            StartCoroutine(Reload());
            return;
        }

        if (Vector3.Distance(transform.position, player.position) <= shootingRange)
        {
            AimAndShoot();
        }
    }

    IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading...");
        yield return new WaitForSeconds(reloadTime);
        currentAmmo = maxAmmo;
        isReloading = false;
    }

    void AimAndShoot()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);

        if (shootingTimer <= 0f)
        {
            Shoot(direction);
            shootingTimer = shootingCooldown;
        }
        else
        {
            shootingTimer -= Time.deltaTime;
        }
    }

    void Shoot(Vector3 direction)
    {
        GameObject bullet = Instantiate(bulletPrefab, shootingPoint.position, Quaternion.identity);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.velocity = direction * bulletSpeed;
        Destroy(bullet, 5f); // Destroy the bullet after 5 seconds to clean up
        currentAmmo--;
    }
}
