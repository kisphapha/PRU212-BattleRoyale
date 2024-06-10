using UnityEngine;
using UnityEngine.Timeline;

public class DestroyOnCollision : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        var rb = GetComponent<Rigidbody2D>();
        if (other.tag == "Solid" || other.tag == "Enermy" || other.tag == "Player")
        {
            PlayerProps playerHealth = other.GetComponent<PlayerProps>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(rb.velocity.magnitude);
            }
            PlayerAIProps enemyHealth = other.GetComponent<PlayerAIProps>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(rb.velocity.magnitude);
            }
            Destroy(gameObject);
        }
        else if(other.tag == "Box") 
        {
            BreakBoxes boxHealth = other.GetComponent<BreakBoxes>();
            if (boxHealth != null)
            {
                Debug.Log(rb.velocity.magnitude);
                boxHealth.TakeDamage(rb.velocity.magnitude);
            }
            Destroy(gameObject);
        }
        else if (other.tag == "Barrel")
        {
            TNTBarrels tntHealth = other.GetComponent<TNTBarrels>();
            if (tntHealth != null)
            {
                Debug.Log(rb.velocity.magnitude);
                tntHealth.TakeDamage(rb.velocity.magnitude);
            }
            Destroy(gameObject);
        }
    }
}
