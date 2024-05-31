using System.Collections.Generic;
using UnityEngine;

public class PickupController : MonoBehaviour
{
    private GameObject pickedObject; // Reference to the currently picked object
    private ShootingController shootingController;
    private PlayerProps master;
    private HealingItem pickedUpHealingItem; // Reference to the currently picked up healing item

    // List to store available weapons
    private List<GameObject> availableWeapons = new List<GameObject>();
    private int currentWeaponIndex = 0; // Index of the currently equipped weapon

    void Start()
    {
        shootingController = GetComponent<ShootingController>();
        master = GetComponent<PlayerProps>();
    }

    void Update()
    {
        CarryTheGun();

        // Check for input to drop or switch the weapon
        if (Input.GetKeyDown(KeyCode.G))
        {
            DropWeapon();
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            SwitchWeapon();
            PickUpHealingItem();
        }
        // Check for input to use the healing item
        else if (Input.GetKeyDown(KeyCode.F) && pickedUpHealingItem != null)
        {
            pickedUpHealingItem.UseHealingItem();
            pickedUpHealingItem = null; // Clear the reference after use
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the colliding object is a weapon
        if (other.CompareTag("PickUpItems"))
        {
            // Add the weapon to the list of available weapons
            availableWeapons.Add(other.gameObject);
        }
        // Check if the colliding object is a healing item
        if (other.CompareTag("Healing"))
        {
            HealingItem healingItem = other.GetComponent<HealingItem>();
            if (healingItem != null)
            {
                healingItem.PickUp(master); // Pass the player reference to the healing item
                pickedUpHealingItem = healingItem; // Keep a reference to the picked-up healing item
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Check if the exiting object is a weapon
        if (other.CompareTag("PickUpItems") || other.CompareTag("Healing"))
        {
            // Remove the weapon from the list of available weapons
            availableWeapons.Remove(other.gameObject);
        }
    }


    public void CarryTheGun()
    {
        if (pickedObject != null)
        {
            float angle = master.GetMouseAngle();
            Quaternion rotation = Quaternion.Euler(0f, 0f, angle - 90);
            Vector3 targetPosition = transform.position + (rotation * Vector3.right * master.offsetDistance);
            pickedObject.transform.position = targetPosition;
            pickedObject.transform.rotation = rotation;
        }
    }
    private void PickUpHealingItem()
    {
        if (pickedUpHealingItem != null)
        {
            pickedUpHealingItem.PickUp(master);
        }
    }

    public void SwitchWeapon()
    {
        // Drop the current weapon
        DropWeapon();

        // Check if there are available weapons
        if (availableWeapons.Count > 0)
        {
            // Move to the next weapon in the list
            currentWeaponIndex = (currentWeaponIndex + 1) % availableWeapons.Count;

            // Equip the new weapon
            EquipWeapon(availableWeapons[currentWeaponIndex]);
        }
    }

    public void EquipWeapon(GameObject weapon)
    {
        pickedObject = weapon;
        pickedObject.transform.SetParent(transform); // Make the player character the parent of the picked object
        Debug.Log("Equipped");
        pickedObject.GetComponent<Collider2D>().enabled = true;
        master.isHeldingGun = true;
        var gun = weapon.GetComponent<GunEntity>();
        master.weapon = gun;
        master.weapon.UpdateAmmoDisplay();
        shootingController.shootCooldown = gun.fireRate;
    }

    public void DropWeapon()
    {
        Debug.Log("Attempting to drop weapon.");
        if (pickedObject != null)
        {
            pickedObject.transform.SetParent(null); // Remove the player character as the parent of the picked object
            Debug.Log("Dropped");
            pickedObject.GetComponent<Collider2D>().enabled = true;
            master.isHeldingGun = false;
            pickedObject = null;
            if (master.weapon != null)
            {
                master.weapon.ResetAmmoDisplay();
                master.weapon = null;
            }
        }
    }
}