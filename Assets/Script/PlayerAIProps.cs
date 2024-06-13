using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAIProps : MonoBehaviour
{
    public string characterName; // Name property
    public float hp = 100; // HP property
    public float offsetDistance = 1.75f; // Constant distance between the player and the picked object
    public GameObject holdingItem;

    private float checkBorderTimer;
    // Start is called before the first frame update
    void Start()
    {
        characterName = "Random Bullshit";

    }

    // Update is called once per frame
    void Update()
    {
        checkBorderTimer += Time.deltaTime;
        if (checkBorderTimer >= 1.5f)
        {
            if (DamageCircle.IsOutsideCircle_Static(gameObject.transform.position))
            {
                TakeDamage(10);
            }
            checkBorderTimer = 0f;
        }
    }

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
