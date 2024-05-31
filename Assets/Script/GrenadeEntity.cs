using Assets.Script;
using UnityEngine;
using UnityEngine.UI;

public class GrenadeEntity : PickableItem
{
    public int maxGrenades = 5;
    public int currentGrenades;
    public float throwRate = 1.5f;
    public float grenadeSpeed = 15f;
    //public Text GrenadeDisplay; // Reference to the UI Text element
    public GameObject grenadePrefab;
    private ThrowingBehavior throwController;
    private float throwTimer = 0f; // Timer to track the cooldown

    private void Start()
    {
        throwController = GetComponent<ThrowingBehavior>();
        currentGrenades = maxGrenades;
        //UpdateGrenadeDisplay(); // Initialize grenade display
    }

    private void Update()
    {
        if (holder != null && holder.holdingItem != null)
        {
            var holderGrenade = holder.holdingItem.GetComponent<GrenadeEntity>();
            if (Input.GetMouseButtonDown(0) && holderGrenade == this) // Assume right-click for throwing grenade
            {
                if (currentGrenades > 0 && throwTimer <= 0f)
                {
                    Debug.Log("Threw");
                    ThrowGrenade();
                    throwTimer = throwRate;
                }
            }
        }
        if (throwTimer > 0f)
        {
            throwTimer -= Time.deltaTime; // Decrease the cooldown timer
        }
    }

    public void ThrowGrenade()
    {
        currentGrenades--;
        Debug.Log("Grenades left: " + currentGrenades);
        throwController.Throw();
        //UpdateGrenadeDisplay(); // Update grenade display after throwing
    }

    //public void UpdateGrenadeDisplay()
    //{
    //    // Check if GrenadeDisplay is assigned
    //    if (GrenadeDisplay != null)
    //    {
    //        GrenadeDisplay.text = "Grenades: " + currentGrenades.ToString();
    //        // Update the UI text with current grenade count
    //    }
    //    else
    //    {
    //        Debug.LogWarning("Grenade display is not assigned!");
    //    }
    //}
}