using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAIProps : MonoBehaviour
{
    public string characterName; // Name property
    public float hp = 100; // HP property
    public float offsetDistance = 1.75f; // Constant distance between the player and the picked object
    public GameObject holdingItem;

    // Start is called before the first frame update
    void Start()
    {
        characterName = "Random Bullshit";

    }

    // Update is called once per frame
    void Update()
    {
        RotateWithMouse();
    }

    private void RotateWithMouse()
    {

    }
    /// <summary>
    /// Get the angle in degree between mouse position and the object.
    /// The 0 degree is count from the bottom (south) direction and increase counterclockwise.
    /// </summary>
    /// <returns></returns>
    public void TakeDamage(float amount)
    {
        hp -= amount;
        Debug.Log(hp);
        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }
}
