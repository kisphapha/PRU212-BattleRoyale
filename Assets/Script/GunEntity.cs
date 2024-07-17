using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class GunEntity : PickableItem
{
    public int maxAmmo = 10;
    public int currentAmmo;
    public float reloadTime = 3f;
    public float fireRate = 10f;
    public float bulletSpeed = 20f;
    public int spriteAngle; //0 độ bắt đầu từ góc 3h và tăng theo ngược chiều kim đồng hồ. Nếu sprite bị nghiêng, chỉnh số này theo góc nghiêng.
    public Text AmmoDisplay; // Reference to the UI Text element
    public GameObject bullet;
    private ShootingBehavior shootingBehavior;
    private float shootTimer = 0f; // Timer to track the cooldown
    public bool canShoot = false;
    private float AiGunTimer = 1f;
    private PhotonView view;
    private void Start()
    {
        Stackable = false;
        spriteAngle = getSpriteAngle();
        shootingBehavior = GetComponent<ShootingBehavior>();
        currentAmmo = maxAmmo;
        if (AmmoDisplay == null)
        {
            AmmoDisplay = GameObject.FindWithTag("AmmoDisplayer")?.GetComponent<Text>();
        }
        view = GetComponent<PhotonView>();
    }

    private void Update()
    {
        if (holder != null)
        {
            var weapon = holder.holdingItem?.GetComponent<GunEntity>();
            if (weapon != null && weapon.currentAmmo > 0 && weapon == this)
            {
                canShoot = true;
            }
            else
            {
                canShoot = false;
            }
        }
        else
        {
            canShoot = false;
        }

        if (holderAI != null)
        {
            var r = new System.Random();
            if (holderAI.holdingItem != null)
            {
                var weapon = holderAI.holdingItem.GetComponent<GunEntity>();
                if (r.Next(30) <= 1 && weapon != null && weapon.currentAmmo > 0 && weapon == this &&
                    holderAI.GetComponent<AIBehavior>().isAttackable)
                {
                    if (AiGunTimer <= 0f)
                    {
                        Shoot();
                        AiGunTimer = fireRate;
                    }
                }
                if (AiGunTimer > 0f)
                {
                    AiGunTimer -= Time.deltaTime; // Decrease the cooldown timer
                }
            }
            
        }

        if (Input.GetMouseButton(0) && canShoot)
        {
            if (shootTimer <= 0f)
            {
                Shoot();
                shootTimer = fireRate;
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
        view.RPC("SyncCurrentAmmo", RpcTarget.OthersBuffered, currentAmmo);
        shootingBehavior.Shoot();
        if (holderAI == null)
        {
            UpdateAmmoDisplay(); // Update ammo display after shooting
        }     

    }
    public void ResetAmmoDisplay()
    {
        shootingBehavior.OnDrop();
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
                case "Uzi":
                    return 45;
                case "MP5":
                    return 30;
                case "Deagle":
                    return 45;
                case "Glock":
                    return 45;
                case "M4":
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

    [PunRPC]
    void SyncCurrentAmmo(int currentAmmo)
    {

        var gun = gameObject.GetComponent<GunEntity>();
        gun.currentAmmo = currentAmmo;
        
    }
}
