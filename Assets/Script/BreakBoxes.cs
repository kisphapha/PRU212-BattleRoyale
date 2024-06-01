using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakBoxes : MonoBehaviour
{
    public float hp = 100;
    public GameObject particles;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TakeDamage(float amount)
    {
        hp -= amount;
        Debug.Log(hp);
        if (hp <= 0)
        {
            Instantiate(particles, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
