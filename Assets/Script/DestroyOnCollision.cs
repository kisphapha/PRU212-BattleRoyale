using UnityEngine;
using UnityEngine.Timeline;

public class DestroyOnCollision : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Solid" || other.tag == "Enermy" || other.tag == "Player")
        {
            PlayerProps playerHealth = other.GetComponent<PlayerProps>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(1);
            }
            Destroy(gameObject);
        }
    }
}
