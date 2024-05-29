using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class GunEntity : MonoBehaviour
{
    public int maxAmmo = 10;
    public int currentAmmo;
    public float reloadTime = 3f;
    public float fireRate = 10f;
    private bool isReloading = false;
    public int spriteAngle = 45;
    public Text AmmoDisplay; // Reference to the UI Text element
    public PickupController pickupController; // Reference to the PickupController

    private void Start()
    {
        currentAmmo = maxAmmo;
        // Find the PickupController in the scene and assign it to pickupController
        pickupController = FindObjectOfType<PickupController>();

        // Check if pickupController is null after attempting to find it
        if (pickupController == null)
        {
            Debug.LogError("PickupController not found in the scene!");
        }
    }

    private void Update()
    {
    }

    public void Shoot()
    {
        currentAmmo--;
        Debug.Log(currentAmmo);
        UpdateAmmoDisplay(); // Update ammo display after shooting
    }

    public void ResetAmmoDisplay()
    {
        // Check if AmmoDisplay is assigned
        if (AmmoDisplay != null)
        {
            AmmoDisplay.text = "Ammo: " ;
        }
        else
        {
            Debug.LogWarning("Ammo display is not assigned!");
        }
    }
    public void UpdateAmmoDisplay()
    {
        // Check if AmmoDisplay is assigned
        if (AmmoDisplay != null)
        {        
            AmmoDisplay.text = "Ammo: " + currentAmmo.ToString();          
            // Update the UI text with current ammo count
        }
        else
        {
            Debug.LogWarning("Ammo display is not assigned!");
        }
    }
}
