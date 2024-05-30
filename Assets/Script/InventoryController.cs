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
    private InventoryDrawer inventoryDrawer;
    // Start is called before the first frame update
    void Start()
    {
        inventoryDrawer = GetComponent<InventoryDrawer>();
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
        if (scrollDelta != 0)
        {
            if (scrollDelta > 0)
            {
                currentSlot--;
                if (currentSlot < 1)
                {
                    currentSlot = maxSlot;
                }
            }
            if (scrollDelta < 0)
            {
                currentSlot++;
                if (currentSlot > maxSlot)
                {
                    currentSlot = 1;
                }
            }
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
            var gun = currentItem.GetComponent<GunEntity>();
            if (gun != null)
            {
                gun.holder = master;
                master.isHeldingGun = true;
                master.weapon = gun;
                master.weapon.UpdateAmmoDisplay();
            }
        }
        inventoryDrawer.UpdateInventoryFrame();
    }
    public int FindAvailableSlot()
    {
        var currentItem = Inventory[currentSlot - 1];
        if (currentItem == null)
        {
            return currentSlot;
        }
        for (int i = 0; i < maxSlot; i++)
        {
            if (Inventory[i] == null)
            {
                return i + 1;
            }
        }
        return 0;
    }
    public bool InventoryAdd(GameObject item)
    {
        int slot = FindAvailableSlot();
        if (slot > 0)
        {
            Inventory[slot - 1] = item;
            inventoryDrawer.UpdateInventoryDisplay();
            handleSlotChange();
            if (slot != currentSlot)
            {
                item.SetActive(false);
            }
            return true;
        }
        else
        {
            return false;
        }
        
    }
    public bool InventoryRemove(int slot)
    {
        if (Inventory[slot] != null)
        {
            Inventory[slot] = null;
            inventoryDrawer.UpdateInventoryDisplay();
            handleSlotChange();
            return true;
        } else
        {
            return false;
        }
        
    }
}
