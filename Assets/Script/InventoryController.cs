using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class InventoryController : MonoBehaviour
{
    public List<GameObject> Inventory = new List<GameObject>();
    public GameObject currentItem;
    public PlayerProps master;
    public int maxSlot = 3;
    public int currentSlot = 1;
    // Start is called before the first frame update
    void Start()
    {
        master = GetComponent<PlayerProps>();
        //setup inventory
        for (int i = 0; i < maxSlot; i++)
        {
            Inventory.Add(null);
        }
    }

    // Update is called once per frame
    void Update()
    {
        float scrollDelta = Input.GetAxis("Mouse ScrollWheel");
        if (scrollDelta > 0 && currentSlot > 1)
        {
            currentSlot--;
            handleSlotChange();
        }
        if (scrollDelta < 0 && currentSlot < maxSlot)
        {
            currentSlot++;
            handleSlotChange();
        }
    }
    public void handleSlotChange()
    {
        if (currentItem != null)
        {
            currentItem.SetActive(false);
            if (master.weapon != null)
            {
                master.weapon.ResetAmmoDisplay();
                master.weapon = null;
            }      
        }
        currentItem = Inventory[currentSlot - 1];
        if (currentItem != null)
        {
            currentItem.SetActive(true);
            master.isHeldingGun = true;
            var gun = currentItem.GetComponent<GunEntity>();
            gun.holder = master;
            master.weapon = gun;
            master.weapon.UpdateAmmoDisplay();
        }
    }
    public int FindAvailableSlot()
    {
        for (int i = 0; i < maxSlot; i++)
        {
            if (Inventory[i] == null)
            {
                return i + 1;
            }
        }
        return 0;
    }

}
