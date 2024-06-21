using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            }
            else
            {
                existItem.Amount++;
                status = 2;
            }

            //inventoryDrawer.UpdateInventoryDisplay();
            if (slot != currentSlot)
            {
                item.SetActive(false);
            }
           
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
            }
            else
            {
                Inventory[slot] = null;
            }
            //inventoryDrawer.UpdateInventoryDisplay();
            //handleSlotChange();
            return true;
        }
        else
        {
            return false;
        }
    }
}
