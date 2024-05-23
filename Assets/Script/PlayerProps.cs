using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProps : MonoBehaviour
{
    public string characterName; // Name property
    public int hp = 100; // HP property
    public bool isHeldingGun = false;
    public float angle = 0f;
    public float offsetDistance = 1.75f; // Constant distance between the player and the picked object


    // Start is called before the first frame update
    void Start()
    {
        var persistentData = FindObjectOfType<PersistentData>();
        characterName = persistentData.playerName;
        Debug.Log(characterName);
    }

    // Update is called once per frame
    void Update()
    {
        RotateWithMouse();
    }

    private void RotateWithMouse()
    {
        angle = GetMouseAngle();
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
    /// <summary>
    /// Get the angle in degree between mouse position and the object.
    /// The 0 degree is count from the bottom (south) direction and increase counterclockwise.
    /// </summary>
    /// <returns></returns>
    public float GetMouseAngle() 
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = transform.position.z; // Assuming the object and mouse are in the same 2D plane
        // Calculate the direction from the object to the mouse position
        Vector3 direction = mousePosition - transform.position;
        // Calculate the angle in degrees
        return Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90;
    }
}
