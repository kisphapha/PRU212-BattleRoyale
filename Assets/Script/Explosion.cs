using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{

    public GameObject explosion;
    public Transform bullet;
    public float colliderRadius = 0.5f;
    public bool collided = false;
    public LayerMask whatToCollideWith;
    // Start is called before the first frame update
    private void FixedUpdate()
    {
        if(explosion == null)
        {
            Debug.LogError("Explosion prefab not assigned.");
            return;
        }

         collided = Physics2D.OverlapCircle(explosion.transform.position, colliderRadius, whatToCollideWith);

        if(collided) {

            Instantiate(explosion, bullet.position, transform.rotation = Quaternion.identity);
            Destroy(gameObject); 
            
            }
        if(!GetComponent<Renderer>().isVisible) Destroy(gameObject);
    }
}
