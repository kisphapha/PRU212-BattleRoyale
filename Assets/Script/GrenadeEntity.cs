using Assets.Script;
using UnityEngine;
using UnityEngine.UI;

public class GrenadeEntity : PickableItem
{
    public float throwRate = 1.5f;
    public float grenadeSpeed = 15f;
     public float explosionRadius = 5.0f;
    public float explosionForce = 700f;
    //public Text GrenadeDisplay; // Reference to the UI Text element
    public GameObject grenadePrefab;
    private ThrowingBehavior throwController;
    public GameObject explosionPrefab;
    private void Start()
    {
        usingCoolDown = throwRate;
        Stackable = true;
        throwController = GetComponent<ThrowingBehavior>();
        //UpdateGrenadeDisplay(); // Initialize grenade display
    }
    public override void ItemEffect()
    {
        Debug.Log("Threw");
        ThrowGrenade();
    }

    void OnCollisionEnter(Collision collision)
    {
    }

    
    public void Explode()
    {
        // Instantiate explosion effect
        Debug.Log("Explode");
        Instantiate(explosionPrefab, transform.position, transform.rotation);

        // Apply explosion force to nearby objects
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider nearbyObject in colliders)
        {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            }
        }

        // Destroy the grenade
        Destroy(gameObject);
    }

    public void ThrowGrenade()
    {
        throwController.Throw();
    }

}