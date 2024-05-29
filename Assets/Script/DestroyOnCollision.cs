using UnityEngine;
using UnityEngine.Timeline;

public class DestroyOnCollision : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Solid" || other.tag == "Enermy" || other.tag == "Player")
        {
            var rb = GetComponent<Rigidbody2D>();
            PlayerProps playerHealth = other.GetComponent<PlayerProps>();
            if (playerHealth != null)
            {
                Debug.Log(rb.velocity.magnitude);
                playerHealth.TakeDamage(rb.velocity.magnitude);
            }
            Destroy(gameObject);
        }
    }
}
