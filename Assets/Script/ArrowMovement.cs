using Photon.Pun;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class ArrowMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private bool stunned = false;

    private Rigidbody2D rb;
    private GameObject parent;
    private PlayerProps player;
    private PhotonView photonView;
    private void Start()
    {
        parent = transform.parent.gameObject;
        player = parent.GetComponent<PlayerProps>();
        rb = parent.GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        photonView = parent.GetComponent<PhotonView>();
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            RotateWithMouse();
            if (!stunned)
            {
                float moveX = Input.GetAxisRaw("Horizontal");
                float moveY = Input.GetAxisRaw("Vertical");

                Vector2 movement = new Vector2(moveX, moveY);
                rb.velocity = movement * moveSpeed;
            }
        }
    }

    private void RotateWithMouse()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var direction = mousePosition - transform.position;
        player.angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90;
        transform.rotation = Quaternion.Euler(Vector3.forward * player.angle);
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