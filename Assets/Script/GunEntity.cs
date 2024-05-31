using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class GunEntity : MonoBehaviour
{
    public int maxAmmo = 10;
    public int currentAmmo;
    public float reloadTime = 3f;
    public float fireRate = 10f;
    public float bulletSpeed = 20f;
    private bool isReloading = false;
    public int spriteAngle; //0 độ bắt đầu từ góc 3h và tăng theo ngược chiều kim đồng hồ. Nếu sprite bị nghiêng, chỉnh số này theo góc nghiêng.
    public Text AmmoDisplay; // Reference to the UI Text element
    public PlayerProps holder;
    public GameObject bullet;
    private ShootingBehavior shootingBehavior;
    private float shootTimer = 0f; // Timer to track the cooldown

    private void Start()
    {
        spriteAngle = getSpriteAngle();
        shootingBehavior = GetComponent<ShootingBehavior>();
        currentAmmo = maxAmmo;
    }

    private void Update()
    {
        if (holder != null)
        {
            if (Input.GetMouseButton(0) && holder.isHeldingGun)
            {
                if (holder.weapon != null && holder.weapon.currentAmmo > 0)
                {
                    if (shootTimer <= 0f)
                    {
                        Shoot();
                        shootTimer = fireRate;
                    }
                }
            }
        }
        if (shootTimer > 0f)
        {
            shootTimer -= Time.deltaTime; // Decrease the cooldown timer
        }
    }

    public void Shoot()
    {
        currentAmmo--;
        Debug.Log(currentAmmo);
        shootingBehavior.Shoot();
        UpdateAmmoDisplay(); // Update ammo display after shooting
    }
    public void ResetAmmoDisplay()
    {
        // Check if AmmoDisplay is assigned
        if (AmmoDisplay != null)
        {
            AmmoDisplay.text = "Ammo: ";
        }
        else
        {
            Debug.LogWarning("Ammo display is not assigned!");
        }
    }
    public void UpdateAmmoDisplay()
    {
        // Check if AmmoDisplay is assigned
        if (AmmoDisplay != null)
        {
            AmmoDisplay.text = "Ammo: " + currentAmmo.ToString();
            // Update the UI text with current ammo count
        }
        else
        {
            Debug.LogWarning("Ammo display is not assigned!");
        }
    }

    public int getSpriteAngle()
    {
        //Cái này để config mấy sprite súng mà bị nghiêng
        var sprite = GetComponent<SpriteRenderer>().sprite;
        if (sprite != null)
        {
            switch (sprite.name)
            {
                case "Ak":
                    return 45;
                case "Pump_shotgun":
                    return 30;
                //TO-DO : Config mấy thăng còn lại, sprite nào mà quay ngang sang bên phải rồi thì khỏi cần.
                default:
                    return 0;
            }
        }
        else
        {
            return 0;
        }

    }
}
