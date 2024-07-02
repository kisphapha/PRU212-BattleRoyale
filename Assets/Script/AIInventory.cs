using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class AIInventory : MonoBehaviour
{
    public List<InventoryItem> Inventory = new List<InventoryItem>();
    public GameObject currentItem;
    public PlayerAIProps master;
    public int maxSlot = 3;
    public int currentSlot = 1;
    // Start is called before the first frame update
    void Start()
    {
        master = GetComponent<PlayerAIProps>();
        //setup inventory
        for (int i = 0; i < maxSlot; i++)
        {
            Inventory.Add(null);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void handleSlotChange()
    {
        if (currentItem != null)
        {
            currentItem.SetActive(false);
            if (master.holdingItem != null)
            {
                master.holdingItem = null;
            }
        }
        currentItem = Inventory[currentSlot - 1]?.GameObject;
        if (currentItem != null)
        {
            currentItem.SetActive(true);
            master.holdingItem = currentItem;
        }
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
                if ((currentPickUpItem.name == pickUpItem.name) && pickUpItem.Stackable)
                {
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
                checkItemType(item,true);
                status = 1;
            }
            else
            {
                checkItemType(item, true);
                existItem.Amount++;
                status = 2;
            }

            if (slot != currentSlot)
            {
                item.SetActive(false);
            } else
            {
                currentItem = item;
            }
            var pickItem = item.GetComponent<PickableItem>();
            if (pickItem != null)
            {
                pickItem.holderAI = master;
                pickItem.inventoryControllerAI = this;
            }
        }
        else
        {
            status = 0;
        }
        return status;
    }
    private void checkItemType(GameObject item, bool isAdd)
    {
        var gun = item.GetComponent<GunEntity>();
        if (gun != null)
        {
            if (isAdd)
            {
                master.gunNumber++;
            } else
            {
                master.gunNumber--;
            }
        } else
        {
            if (isAdd)
            {
                master.otherItemNumber++;
            }
            else
            {
                master.otherItemNumber--;
            }
        }
    }
    public bool InventoryRemove(int slot, bool isUse = false)
    {
        if (Inventory[slot] != null)
        {
            var item = Inventory[slot];
            if (item.Amount > 1)
            {
                Inventory[slot].Amount--;
            }
            else
            {
                if (isUse)
                {
                    Destroy(item.GameObject);
                }
                Inventory[slot] = null;
            }
            checkItemType(item.GameObject, true);
            //inventoryDrawer.UpdateInventoryDisplay();
            handleSlotChange();
            return true;
        }
        else
        {
            return false;
        }
    }
    public void SwitchTo(int slot)
    {
        currentSlot = slot;
        handleSlotChange();
    }
    public int FindWeaponSlot()
    {
        for (var i = 0; i < maxSlot; i++)
        {
            if (Inventory[i] != null)
            {
                var gun = Inventory[i].GameObject.GetComponent<GunEntity>();
                if (gun != null)
                {
                    return i;
                }
            }
        }
        return -1;
    }
    public int FindGrenadeSlot()
    {
        for (var i = 0; i < maxSlot; i++)
        {
            if (Inventory[i] != null)
            {
                var grenade = Inventory[i].GameObject.GetComponent<GrenadeEntity>();
                if (grenade != null)
                {
                    return i;
                }
            }
        }
        return -1;
    }
    public int FindHealerSlot()
    {
        for (var i = 0; i < maxSlot; i++)
        {
            if (Inventory[i] != null)
            {
                var healer = Inventory[i].GameObject.GetComponent<HealingItem>();
                if (healer != null)
                {
                    return i;
                }
            }
        }
        return -1;
    }
    public bool IsFull()
    {
        for (var i = 0; i < maxSlot; i++)
        {
            if (Inventory[i] == null)
            {
                return false;
            }
        }
        return true;
    }

    public GameObject GetCurrentItem()
    {
        return Inventory[currentSlot - 1]?.GameObject;
    }
}
