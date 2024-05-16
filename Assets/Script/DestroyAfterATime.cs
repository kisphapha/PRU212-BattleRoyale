using UnityEngine;

public class DestroyAfterATime : MonoBehaviour
{
    public float destroyDelay = 3f; // Delay in seconds before destroying the object

    private void Start()
    {
        Destroy(gameObject, destroyDelay);
    }
}
