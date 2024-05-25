using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDamage : MonoBehaviour
{
    
    public int damage = 1;
    //public PlayerHealth playerHealth;
    // Start is called before the first frame update
    void Start()
    {
    }

    private void OnCollisionEnter2D(Collider2D collision)
    {
        PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
        }
        Destroy(gameObject);
    }
}
