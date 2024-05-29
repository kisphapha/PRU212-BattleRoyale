using UnityEngine;
using UnityEngine.UI;

namespace Assets.Script
{
    public class Sniper : Guns
    {
        public GameObject bulletPrefab; // Assign your bullet prefab in the inspector
        public Transform firePoint; // The point where bullets are instantiated
        public float bulletSpeed = 20f; // High bullet speed

        private float nextTimeToFire = 0f;

        protected override void Start()
        {
            base.Start();
            maxAmmo = 3; // Each magazine can shoot 3 times
            currentAmmo = maxAmmo;
            fireRate = 1f; // Low fire rate, adjust as needed

            // Load and assign the AWP sprite
            gunSprite = Resources.Load<Sprite>("AWP");
            if (gunSprite != null)
            {
                // Assuming you have an Image component to display the sprite
                GetComponent<SpriteRenderer>().sprite = gunSprite;
            }
            else
            {
                Debug.LogWarning("AWP sprite not found in Resources!");
            }
        }

        public override void Shoot()
        {
            if (Time.time >= nextTimeToFire)
            {
                if (currentAmmo > 0)
                {
                    Debug.Log("Sniper shooting...");
                    currentAmmo--;
                    UpdateAmmoDisplay();
                    nextTimeToFire = Time.time + 1f / fireRate;

                    // Instantiate and shoot the bullet
                    GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
                    Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                    rb.velocity = firePoint.right * bulletSpeed;

                    // Add logic for no ricochet if necessary
                }
                else
                {
                    Debug.Log("Out of ammo, reload needed.");
                }
            }
        }

        public new void UpdateAmmoDisplay()
        {
            if (AmmoDisplay != null)
            {
                AmmoDisplay.text = "Ammo: " + currentAmmo.ToString();
            }
            else
            {
                Debug.LogWarning("Ammo display is not assigned!");
            }
        }
    }
}
