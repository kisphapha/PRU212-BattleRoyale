using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPIckUp : MonoBehaviour
{
    public GameObject pickUp;
    private GameObject pickedObject; // Reference to the currently picked object
    private PlayerAIProps master;
    private HealingItem pickedUpHealingItem; // Reference to the currently picked up healing item
    private AIChase chase;
    // List to store available weapons
    private List<GameObject> availableWeapons = new List<GameObject>();
    private InventoryController inventoryController;
    private int currentWeaponIndex = 0; // Index of the currently equipped weapon
                                        // Start is called before the first frame update

    public float detectionRadius = 1f;
    private float distance = 1f;
    public float speed = 1f;
    bool active = false;
    public int count = 3;
    void Start()
    {
        inventoryController = GetComponent<InventoryController>();
        master = GetComponent<PlayerAIProps>();
        chase.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        CarryTheItem();
        distance = Vector2.Distance(transform.position, pickUp.transform.position);
        Vector2 direction = pickUp.transform.position - transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (!active)
        {
            transform.position = Vector2.MoveTowards(this.transform.position, pickUp.transform.position, speed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(Vector3.forward * angle);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the colliding object is a weapon
        if (other.CompareTag("PickUpItems"))
        {
            count--;
            // Add the weapon to the list of available weapons
            availableWeapons.Add(other.gameObject);

            EquipWeapon(other.gameObject);
            if (count <= 0)
            {
                active = true;
            }
            other.gameObject.tag = "Untagged";
            //this.enabled = false;
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

            Vector3 direction = master.transform.position - transform.position;
            direction.z = master.transform.position.z;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90;
            Quaternion itemRotation = Quaternion.Euler(0f, 0f, angle - deviationAngle - 90);
            Quaternion masterRotation = Quaternion.Euler(0f, 0f, angle  - 90);
            Vector3 targetPosition = transform.position + (masterRotation * Vector3.right * itemSpriteLength/2);
            pickedObject.transform.position = targetPosition;
            pickedObject.transform.rotation = itemRotation;
        }
    }
    public void EquipWeapon(GameObject weapon)
    {
        pickedObject = weapon;
        pickedObject.transform.SetParent(transform); // Make the player character the parent of the picked object
        Debug.Log("Equipped");
        pickedObject.GetComponent<Collider2D>().enabled = false;
        //CarryTheItem();
    }
   
}
