using Assets.Script;
using UnityEngine;
using UnityEngine.UI;

public class GrenadeEntity : PickableItem
{
    public float throwRate = 1.5f;
    public float grenadeSpeed = 15f;
    public float explosionRadius = 5.0f;
    public float explosionDuration = 0.25f;
    public float explosionDelay = 2.5f;
    public float explosionForce = 700f;
    //public Text GrenadeDisplay; // Reference to the UI Text element
    public GameObject grenadePrefab;
    public GameObject smokeParticlePrefab;
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

    public void ThrowGrenade()
    {
        var grenade = throwController.Throw();
        var grenadeThrowing = grenade.GetComponent<ExplodeOnCollision>();
        if (grenadeThrowing != null)
        {
            grenadeThrowing.Setup(explosionRadius,explosionForce,explosionDuration,explosionDelay);
            grenadeThrowing.smokeParticlePrefab = smokeParticlePrefab;
            grenadeThrowing.explosionPrefab = explosionPrefab;
        }
    }

}