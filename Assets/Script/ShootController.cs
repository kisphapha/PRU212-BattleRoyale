using System;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class ShootingController : MonoBehaviour
{
    public GameObject Bullet; // Reference to the bullet prefab
    private PlayerProps master;
    private float coolDown = 1;
    private float shootCooldown = 0.25f; // Cooldown time in seconds
    private float shootTimer = 0f; // Timer to track the cooldown
    public float bulletSpeed = 20f; // Speed of the bullet

    private void Start()
    {
        master =  GetComponent<PlayerProps>();
    }
    private void Update()
    {
        if (Input.GetMouseButton(0) && master.isHeldingGun)
        {
            if (shootTimer <= 0f)
            {
                Shoot();
                shootTimer = shootCooldown; // Reset the cooldown timer
            }
        }

        if (shootTimer > 0f)
        {
            shootTimer -= Time.deltaTime; // Decrease the cooldown timer
        }
    }

    private void Shoot()
    {
        int randomAngle = UnityEngine.Random.Range(-10, 10);
        //Gun offset để xác định khoảng cách để tạo viên đạn (đan nên tạo ra từ đầu súng thay vì
        //từ giữa súng)
        float gunOffset = 4f;

        //Tạo 1 phép quay để quay chiều z của 1 object về randomAngle (từ -10 tới 10)
        //Để tạo hiệu ứng đạn lạcs
        Quaternion rotationRandom = Quaternion.Euler(0f, 0f, randomAngle);

        //Để tạo hiệu ứng object quay thì phải transform rotation nó trên trục z
        //Tuy nhiên sprite của player thì quay xuống còn sprite súng thì quay sang ngang lên bị lệch
        //góc 90 độ nên phải tạo 2 cái quaternion khác nhau như dưới
        Quaternion rotationOfGun = Quaternion.Euler(0f, 0f, master.angle - 90);
        Quaternion rotationOfMaster = Quaternion.Euler(0f, 0f, master.angle) ;

        //Tạo 1 vị trí mới từ vị trị của Player, cộng thêm chiều dài offset về hướng mà súng quay tới
        Vector3 targetPosition = transform.position + (rotationOfGun * Vector3.right * gunOffset);
        //Xác định vị trí mà viên đan sẽ bay tới, bằng vị trí đối của trục xanh và phép quay random
        Vector3 direction = rotationRandom * transform.up * -1;

        // Instantiate the bullet prefab at the player's position and rotation (Tạo 1 object từ prefab)
        // Tạo từ vị trí đầu súng (nhờ offset) và quay theo hướng của Player vì sprite của đạn cũng
        // quay xuống dưới
        GameObject bullet = Instantiate(Bullet, targetPosition, rotationOfMaster);

        // Get the bullet's rigidbody component
        Rigidbody2D bulletRigidbody = bullet.GetComponent<Rigidbody2D>();

        // Set the bullet's velocity based on the player's rotation
        bulletRigidbody.velocity = direction * bulletSpeed;
    }
}
