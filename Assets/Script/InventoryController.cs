using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public List<InventoryItem> Inventory = new List<InventoryItem>();
    public GameObject currentItem;
    public PlayerProps master;
    public int maxSlot = 3;
    public int currentSlot = 1;
    private InventoryDrawer inventoryDrawer;
    // Start is called before the first frame update//
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
            if (master.holdingItem != null)
            {
                var weapon = master.holdingItem.GetComponent<GunEntity>();
                if (weapon != null)
                {
                    weapon.ResetAmmoDisplay();
                }
                master.holdingItem = null;
            }      
        }
        currentItem = Inventory[currentSlot - 1]?.GameObject;
        if (currentItem != null)
        {
            currentItem.SetActive(true);
            
            master.holdingItem = currentItem;
            var gun = currentItem.GetComponent<GunEntity>();
            if (gun != null)
            {
                master.isHeldingGun = true;
                gun.UpdateAmmoDisplay();
            }
        }
        inventoryDrawer.UpdateInventoryFrame();
    }
    public int FindAvailableSlot(GameObject item)
    {
        //Find object of same type if stackable
        for (int i = 0; i < maxSlot; i++)
        {
            var pickUpItem = item.GetComponent<PickableItem>();
            if (Inventory[i] != null)
            {
                var currentPickUpItem = Inventory[i].GameObject.GetComponent<PickableItem>();
                //Debug.Log($"{item.name} is stackable : {pickUpItem.Stackable}");
                if ((currentPickUpItem.name == pickUpItem.name) && pickUpItem.Stackable)
                {
                    //Debug.Log($"{Inventory[i].GameObject.name} and {item.name} are the same prefab");
                    return i + 1;
                }
            }        
        }
        //If currentItem is empty
        var currentItem = Inventory[currentSlot - 1];
        if (currentItem == null)
        {
            return currentSlot;
        }
        //Find the first null slot in the inventory
        for (int i = 0; i < maxSlot; i++)
        {
            if (Inventory[i] == null)
            {
                return i + 1;
            }
        }
        return 0;
    }
    public int InventoryAdd(GameObject item)
    {
        int status; //0 = Failed, 1 = Success add into empty slot, 2 = Success add stacked
        int slot = FindAvailableSlot(item);
        if (slot > 0)
        {
            var existItem = Inventory[slot - 1];
            if (existItem == null)
            {
                Inventory[slot - 1] = new InventoryItem
                {
                    GameObject = item,
                    Amount = 1
                };
                status = 1;
            } else
            {
                existItem.Amount++;
                status = 2;
            }

            inventoryDrawer.UpdateInventoryDisplay();
            if (slot != currentSlot)
            {
                item.SetActive(false);
            }
            var pickItem = item.GetComponent<PickableItem>();
            if (pickItem != null)
            {
                pickItem.holder = master;
                pickItem.inventoryController = this;
            }
            handleSlotChange();
        }
        else
        {
            status = 0;
        }
        return status;
    }
    public bool InventoryRemove(int slot)
    {
        if (Inventory[slot] != null)
        {
            if (Inventory[slot].Amount > 1)
            {
                Inventory[slot].Amount--;
            } else
            {
                Inventory[slot] = null;
            }
            inventoryDrawer.UpdateInventoryDisplay();
            handleSlotChange();
            return true;
        } else
        {
            return false;
        }
    }
}
