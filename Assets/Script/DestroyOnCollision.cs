using UnityEngine;
using UnityEngine.Timeline;

public class DestroyOnCollision : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Solid")
        {
            Destroy(gameObject);
        }
    }
}
