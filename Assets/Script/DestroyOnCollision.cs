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
                Debug.Log(rb.velocity.magnitude);
                playerHealth.TakeDamage(rb.velocity.magnitude);
            }
            var gernade = GetComponent<GrenadeEntity>();
            Debug.Log("Gernade in Destroy: " + gernade);
            if (gernade != null)
            {
                gernade.Explode();
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
        }else if (other.tag == "Barrel")
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
