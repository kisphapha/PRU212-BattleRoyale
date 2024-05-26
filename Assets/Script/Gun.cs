using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class Gun : MonoBehaviour
{
    public int maxAmmo = 10;
    private int currentAmmo;
    public float reloadTime = 3f;
    public float fireRate = 10f;
    private bool isReloading = false;
    public Text AmmoDisplay; // Reference to the UI Text element
    private bool isFiring = false; // Flag to track if the fire button is being held down
    private bool isGunPickedUp = false; // Flag to track if the gun is picked up
    public PickupController pickupController; // Reference to the PickupController
    private ShootingController shootingController;

    void Start()
    {

        currentAmmo = maxAmmo;
        UpdateAmmoDisplay(); // Update the initial ammo display

        // Find the PickupController in the scene and assign it to pickupController
        pickupController = FindObjectOfType<PickupController>();

        // Check if pickupController is null after attempting to find it
        if (pickupController == null)
        {
            Debug.LogError("PickupController not found in the scene!");
        }
    }

    void Update()
    {
        if (isReloading)
            return;

        if (Input.GetMouseButtonDown(0) && currentAmmo > 0 && isGunPickedUp)
        {
            isFiring = true;
            StartCoroutine(FireContinuous());
        }

        if (Input.GetMouseButtonDown(1))
        {
            isFiring = false;
        }

        if (Input.GetMouseButtonDown(0) && currentAmmo <= 0 && isGunPickedUp)
        {
            Reload();
        }
    }

    void Shoot()
    {
        if (!isGunPickedUp)
            return; // Don't shoot if the gun is not picked up

        currentAmmo--;
        Debug.Log("Shooting...");
        UpdateAmmoDisplay(); // Update ammo display after shooting
    }

    IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading...");

        yield return new WaitForSeconds(reloadTime);

        currentAmmo = maxAmmo;
        isReloading = false;
        UpdateAmmoDisplay(); // Update ammo display after reloading
    }

    void UpdateAmmoDisplay()
    {
        // Check if AmmoDisplay is assigned
        if (AmmoDisplay != null)
        {
            // Update the UI text with current ammo count
            AmmoDisplay.text = "Ammo: " + currentAmmo.ToString();
            Debug.Log("this is shoot: ");
        }
        else
        {
            Debug.LogWarning("Ammo display is not assigned!");
        }
    }

    // Call this method when the gun is picked up
    public void OnPickup()
    {
        isGunPickedUp = true;
        Debug.Log("this is equip in Gun module");
        SetAmmoCount(maxAmmo); // Set the bullet count to max ammo when the gun is picked up
        UpdateAmmoDisplay(); // Update the ammo display
        gameObject.SetActive(false); // Disable the gun GameObject when picked up
        if (pickupController != null)
        {
            pickupController.EquipWeapon(gameObject); // Call EquipWeapon method in PickupController
        }
    }

    // Call this method when the gun is dropped
    public void OnDrop()
    {
        isGunPickedUp = false;
        gameObject.SetActive(true); // Enable the gun GameObject when dropped
        if (pickupController != null)
        {
            pickupController.DropWeapon(); // Call DropWeapon method in PickupController
        }
    }

    public void SetAmmoCount(int count)
    {
        currentAmmo = count;
    }

    IEnumerator FireContinuous()
    {
        // Fire continuously while the fire button is being held down
        while (isFiring && currentAmmo > 0)
        {
            Shoot();
            yield return new WaitForSeconds(fireRate);
        }
    }
}
