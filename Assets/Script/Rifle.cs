using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Script

{
  public class Rifle : Guns
{
    public override void Shoot()
    {
        if (currentAmmo > 0)
        {
            Debug.Log("Rifle shooting...");
            currentAmmo--;
            UpdateAmmoDisplay();
            // Implement the specific shooting logic for Rifle here
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
    }
}
