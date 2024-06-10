using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class ArrowMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private bool stunned = false;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!stunned)
        {
            float moveX = Input.GetAxisRaw("Horizontal");
            float moveY = Input.GetAxisRaw("Vertical");

            Vector2 movement = new Vector2(moveX, moveY);
            rb.velocity = movement * moveSpeed;
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