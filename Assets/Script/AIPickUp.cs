using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPickUp : MonoBehaviour
{
    public GameObject pickUp;
    public CustomTrigger triggerPickUp;
    //public CustomTrigger triggerPlayer;
    private GameObject pickedObject; // Reference to the currently picked object
    private PlayerAIProps master;
    private AIBehavior chase;
    // List to store available weapons
    private AIInventory inventoryController;
    private int currentWeaponIndex = 0; // Index of the currently equipped weapon
                                        // Start is called before the first frame update

    public float detectionRadius = 1f;
    private float distance = 1f;
    public float speed = 1f;
    bool active = false;
    public int count = 3;

    void Start()
    {
        inventoryController = GetComponent<AIInventory>();
        master = GetComponent<PlayerAIProps>();
        chase = GetComponent<AIBehavior>();
        //chase.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        CarryTheItem();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the colliding object is a weapon
        if (other.CompareTag("PickUpItems"))
        {
            if (inventoryController != null)
            {
                // Move to the next weapon in the list
                var weapon = other.gameObject;
                // Equip the new weapon
                var result = inventoryController.InventoryAdd(weapon);
                if (result == 1)
                {
                    EquipWeapon(weapon);
                }
                else if (result == 2)
                {
                    Destroy(weapon);
                }              
            }
            //Debug.Log("should pick up");

            //EquipWeapon(other.gameObject);
            //if (count <= 0)
            //{
            //    active = true;
            //}
            //other.gameObject.tag = "Untagged";
            //this.enabled = false;
        }
    }


    public void CarryTheItem()
    {
        var item = inventoryController.Inventory[inventoryController.currentSlot - 1];
        if (item != null)
        {
            var gun = item.GameObject.GetComponent<GunEntity>();
            var deviationAngle = 0;
            if (gun != null)
            {
                deviationAngle = gun.spriteAngle;
            }
            float itemSpriteLength = item.GameObject.GetComponent<SpriteRenderer>().sprite.bounds.size.x
                * item.GameObject.transform.lossyScale.x;

            Vector3 direction = master.transform.position - transform.position;
            direction.z = master.transform.position.z;
            float angle = chase.angle;
            Quaternion itemRotation = Quaternion.Euler(0f, 0f, angle - deviationAngle);
            Quaternion masterRotation = Quaternion.Euler(Vector3.forward * angle);
            Vector3 targetPosition = transform.position + (masterRotation * Vector3.right * itemSpriteLength/2);
            item.GameObject.transform.position = targetPosition;
            item.GameObject.transform.rotation = itemRotation;
        }
    }
    public void EquipWeapon(GameObject weapon)
    {
        weapon.transform.SetParent(transform); // Make the player character the parent of the picked object
        weapon.GetComponent<Collider2D>().enabled = false;
    }
   
}
