using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Script
{

    public abstract class Guns : MonoBehaviour
    {
        public int maxAmmo;
        public int currentAmmo;
        public float reloadTime;
        public float fireRate;
        public Sprite gunSprite;
        public string gunName;
        public Text AmmoDisplay;

        protected virtual void Start()
        {
            currentAmmo = maxAmmo;
            UpdateAmmoDisplay();
        }

        public abstract void Shoot();

        public virtual void Reload()
        {
            // Base reload logic, can be overridden in derived classes if needed
            currentAmmo = maxAmmo;
            UpdateAmmoDisplay();
        }

        public void UpdateAmmoDisplay()
        {
            if (AmmoDisplay != null)
            {
                AmmoDisplay.text = "Ammo: " + currentAmmo.ToString();
            }
        }
    }
}
