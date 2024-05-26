using UnityEngine;

public class PickupController : MonoBehaviour
{
    private GameObject pickedObject; // Reference to the currently picked object
    private PlayerProps master;
    // Start is called before the first frame update
    void Start()
    {
        master = GetComponent<PlayerProps>();
    }

    // Update is called once per frame
    void Update()
    {
        CarryTheGun();

        // Check for input to drop or pick up/switch the weapon
        if (Input.GetKeyDown(KeyCode.G))
        {
            DropWeapon();
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            if (!master.isHeldingGun)
            {
                EquipWeapon(); // Equip a weapon if none is held
            }
            else
            {
                SwitchWeapon(); // Attempt to switch weapons if one is already held
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // No changes needed here since we're handling everything in Update()
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

    public void EquipWeapon(GameObject weapon = null)
    {
        if (weapon == null)
        {
            // Find the nearest weapon to equip
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 3f);
            foreach (var collider in colliders)
            {
                if (collider.CompareTag("PickUpItems"))
                {
                    weapon = collider.gameObject;
                    break;
                }
            }
        }

        if (weapon != null)
        {
            pickedObject = weapon;
            pickedObject.transform.SetParent(transform); // Make the player character the parent of the picked object
            Debug.Log("Equipped");
            // Enable the object's collider and renderer to make it visually appear
            pickedObject.GetComponent<Collider2D>().enabled = true;
            master.isHeldingGun = true;
        }
    }

    public void DropWeapon()
    {
        Debug.Log("Attempting to drop weapon.");

        if (pickedObject != null)
        {
            pickedObject.transform.SetParent(null); // Remove the player character as the parent of the picked object
            Debug.Log("Dropped");
            // Enable the object's collider and renderer to make it visually reappear
            pickedObject.GetComponent<Collider2D>().enabled = true;
            master.isHeldingGun = false;
            pickedObject = null;
        }
    }

    public void SwitchWeapon()
    {
        // First, drop the current weapon
        DropWeapon();

        // Then, try to equip a new weapon
        EquipWeapon();
    }
}
