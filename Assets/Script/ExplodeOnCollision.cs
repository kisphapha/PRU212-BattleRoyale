using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeOnCollision : MonoBehaviour
{
    public GameObject explosionPrefab;
    private float explosionRadius;
    private float explosionForce;
    private float explosionDuration; // Duration for explosion effect to stay in scene
    private float explosionDelay;
    public int maxColliders = 10; // Limit the number of colliders processed
    private void Start()
    {
        // Start the explosion countdown
        StartCoroutine(ExplosionCountdown());
    }
    public void Setup(float explosionRadius, float explosionForce, float explosionDuration, float explosionDelay)
    {
        this.explosionRadius = explosionRadius;
        this.explosionForce = explosionForce;
        this.explosionDuration = explosionDuration;
        this.explosionDelay = explosionDelay;
    }
    //private void OnTriggerEnter2D(Collider2D other)
    //{
    //    if (other.tag == "Solid" || other.tag == "Enermy" || other.tag == "Player")
    //    {
    //        Explode();
    //    }
    //}

    private IEnumerator ExplosionCountdown()
    {
        // Wait for the specified delay before exploding
        yield return new WaitForSeconds(explosionDelay);
        Explode();
    }

    private void Explode()
    {
        // Instantiate explosion effect
        Debug.Log("Explode");

        if (explosionPrefab == null)
        {
            Debug.LogError("Explosion prefab not assigned.");
            return;
        }

        GameObject explosion = Instantiate(explosionPrefab, transform.position, transform.rotation);

        // Apply explosion force to nearby objects
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        Debug.Log("Number of colliders detected: " + colliders.Length);

        int processedCount = 0;
        foreach (Collider2D nearbyObject in colliders)
        {
            if (processedCount >= maxColliders)
            {
                Debug.LogWarning("Exceeded max colliders limit. Skipping the rest.");
                break;
            }

            Rigidbody2D rb = nearbyObject.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 explosionDirection = nearbyObject.transform.position - transform.position;
                rb.AddForce(explosionDirection.normalized * explosionForce);
            }
            processedCount++;
        }

        // Destroy the grenade
        Destroy(gameObject);

        // Destroy the explosion effect after a delay
        Destroy(explosion, explosionDuration);
    }
}
