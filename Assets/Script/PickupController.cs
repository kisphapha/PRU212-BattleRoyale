using System.Collections.Generic;
using UnityEngine;

public class PickupController : MonoBehaviour
{
    private GameObject pickedObject; // Reference to the currently picked object
    private PlayerProps master;
    private HealingItem pickedUpHealingItem; // Reference to the currently picked up healing item

    // List to store available weapons
    private List<GameObject> availableWeapons = new List<GameObject>();
    private InventoryController inventoryController;
    private int currentWeaponIndex = 0; // Index of the currently equipped weapon

    void Start()
    {
        inventoryController = GetComponent<InventoryController>();
        master = GetComponent<PlayerProps>();
    }

    void Update()
    {
        CarryTheItem();

        // Check for input to drop or switch the weapon
        if (Input.GetKeyDown(KeyCode.G))
        {
            DropWeapon();
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            SwitchWeapon();
            //PickUpHealingItem();
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


    public void CarryTheItem()
    {
        if (pickedObject != null)
        {
            var gun = pickedObject.GetComponent<GunEntity>();
            var deviationAngle = 0;
            if (gun != null)
            {
                deviationAngle = gun.spriteAngle;
            }
            float itemSpriteLength = pickedObject.GetComponent<SpriteRenderer>().sprite.bounds.size.x 
                * pickedObject.transform.lossyScale.x;
            float angle = master.GetMouseAngle();
            Quaternion itemRotation = Quaternion.Euler(0f, 0f, angle - deviationAngle - 90);
            Quaternion masterRotation = Quaternion.Euler(0f, 0f, angle - 90);
            Vector3 targetPosition = transform.position + (masterRotation * Vector3.right * itemSpriteLength/2);
            pickedObject.transform.position = targetPosition;
            pickedObject.transform.rotation = itemRotation;
        }
    }

    public void SwitchWeapon()
    {
        // Drop the current weapon
        //DropWeapon();
        if (inventoryController != null)
        {
            // Check if there are available weapons
            if (availableWeapons.Count > 0)
            {
                // Move to the next weapon in the list
                currentWeaponIndex = (currentWeaponIndex + 1) % availableWeapons.Count;
                var weapon = availableWeapons[currentWeaponIndex];
                // Equip the new weapon
                var result = inventoryController.InventoryAdd(weapon);
                if (result == 1)
                {
                    EquipWeapon(weapon);
                } else if (result == 2)
                {
                    DestroyItem(weapon);
                }
            }
        }
    }

    public void EquipWeapon(GameObject weapon)
    {
        pickedObject = weapon;
        pickedObject.transform.SetParent(transform); // Make the player character the parent of the picked object
        Debug.Log("Equipped");
        pickedObject.GetComponent<Collider2D>().enabled = false;
    }
    public void DestroyItem(GameObject gameObject)
    {
        Destroy(gameObject);
    }
    public void DropStackedItem(GameObject gameObject, GameObject sample)
    {
        var item = Instantiate(gameObject, transform.position, transform.rotation);
        item.transform.localScale = sample.transform.lossyScale;
        item.name = sample.name;
    }
    public void DropWeapon()
    {
        Debug.Log("Attempting to drop weapon.");
        var item = inventoryController.Inventory[inventoryController.currentSlot - 1];
        if (item != null)
        {
            if (item.Amount > 1)
            {
                DropStackedItem(item.GameObject, master.holdingItem);
                inventoryController.InventoryRemove(inventoryController.currentSlot - 1);
            }
            else {
                inventoryController.InventoryRemove(inventoryController.currentSlot - 1);
                item.GameObject.SetActive(true);
                item.GameObject.transform.SetParent(null); // Remove the player character as the parent of the picked object
                Debug.Log("Dropped");
                item.GameObject.GetComponent<Collider2D>().enabled = true;
                pickedObject = null;
                master.isHeldingGun = false;
                var cuurentItem = master.holdingItem?.GetComponent<PickableItem>();
                if (master.holdingItem != null)
                {
                    if (cuurentItem != null)
                    {
                        cuurentItem.holder = null;
                        cuurentItem.inventoryController = null;
                    }
                    var weapon = master.holdingItem.GetComponent<GunEntity>();
                    if (weapon != null)
                    {
                        weapon.ResetAmmoDisplay();
                    }
                    master.holdingItem = null;
                }
            }
        }
    }
}