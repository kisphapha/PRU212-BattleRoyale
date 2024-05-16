using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class PickupController : MonoBehaviour
{
    private GameObject pickedObject; // Reference to the currently picked object
    private PlayerProps master;
    // Start is called before the first frame update
    void Start()
    {
        master =  GetComponent<PlayerProps>();
    }

    // Update is called once per frame
    void Update()
    {
        CarryTheGun();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collided object is the one that can be picked up
        if (other.tag == "PickUpItems")
        {
            pickedObject = other.gameObject;
            pickedObject.transform.SetParent(transform); // Make the player character the parent of the picked object
            Debug.Log("Pickked up");
            // Disable the object's collider and renderer to make it visually disappear
            pickedObject.GetComponent<Collider2D>().enabled = false;
            master.isHeldingGun = true;
        }
    }
    private void CarryTheGun()
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
    
}
