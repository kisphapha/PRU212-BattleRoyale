using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIChase : MonoBehaviour
{
    public GameObject player;
    public float speed;
    public float distanceBetween;
    private bool stunned;

    private float distance;
    Vector2 direction;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!stunned && player != null)
        {
            distance = Vector2.Distance(transform.position, player.transform.position);
            direction = player.transform.position - transform.position;
            direction.Normalize();
            transform.LookAt(player.transform);
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            if (distance < distanceBetween)
            {
                rb.velocity = direction * speed;
                //transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);
                transform.rotation = Quaternion.Euler(Vector3.forward * angle);

            }
        }     
    }

    public void Stunned(float duration)
    {
        StartCoroutine(StunnedCoroutine(duration));
    }

    private IEnumerator StunnedCoroutine(float duration)
    {
        stunned = true;
        yield return new WaitForSeconds(duration);
        stunned = false;
    }

}
