using System.Collections.Generic;
using UnityEngine;

public class PickupController : MonoBehaviour
{
    private GameObject pickedObject; // Reference to the currently picked object
    private PlayerProps master;

    // List to store available weapons
    private List<GameObject> availableWeapons = new List<GameObject>();
    private int currentWeaponIndex = 0; // Index of the currently equipped weapon

    void Start()
    {
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
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Check if the exiting object is a weapon
        if (other.CompareTag("PickUpItems"))
        {
            // Remove the weapon from the list of available weapons
            availableWeapons.Remove(other.gameObject);
        }
    }

    public void CarryTheGun()
    {
        if (pickedObject != null)
        {
            var gun = pickedObject.GetComponent<GunEntity>();
            if (gun != null)
            {
                float angle = master.GetMouseAngle();
                Quaternion gunRotation = Quaternion.Euler(0f, 0f, angle - gun.spriteAngle - 90);
                Quaternion masterRotation = Quaternion.Euler(0f, 0f, angle - 90);
                Vector3 targetPosition = transform.position + (masterRotation * Vector3.right * master.offsetDistance);
                pickedObject.transform.position = targetPosition;
                pickedObject.transform.rotation = gunRotation;
            }          
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
        gun.holder = master;
        master.weapon = gun;
        master.weapon.UpdateAmmoDisplay();
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
                master.weapon.holder = null;
                master.weapon.ResetAmmoDisplay();
                master.weapon = null;
            }
        }
    }
}
