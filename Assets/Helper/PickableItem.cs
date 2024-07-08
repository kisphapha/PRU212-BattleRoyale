using UnityEngine;

public class PickableItem : MonoBehaviour
{
    public PlayerProps holder;
    public PlayerAIProps holderAI;
    public InventoryController inventoryController;
    public AIInventory inventoryControllerAI;
    public bool Stackable;
    private float timer = 0f; // Timer to track the cooldown
    public float usingCoolDown = 0.5f;
    //public string Type;
    private void Update()
    {
        if (holder != null && Stackable)
        {
            if (Input.GetMouseButton(0) && holder.holdingItem != null)
            {
                var pickUpItem = holder.holdingItem.GetComponent<PickableItem>();
                if (timer <= 0f && pickUpItem == this)
                {
                    UseStackableItem();
                    ItemEffect();
                    timer = usingCoolDown;
                }
            }
        }


        if (holderAI != null && Stackable)
        {
            var r = new System.Random();
            if (r.Next(30) <= 1 && holderAI.holdingItem != null)
            {
                var pickUpItem = holderAI.holdingItem.GetComponent<PickableItem>();
                if (timer <= 0f && pickUpItem == this)
                {
                    UseStackableItemAsAI();
                    ItemEffect();
                    timer = usingCoolDown;
                }
            }
        }
        if (timer > 0f)
        {
            timer -= Time.deltaTime; // Decrease the cooldown timer
        }


    }
    public void UseStackableItem()
    {
        if (holder != null && inventoryController != null)
        {
            for (var i = 0; i < inventoryController.maxSlot; i++)
            {
                var currentSlotItem = inventoryController?.Inventory[i];
                var pickUpItem = currentSlotItem?.GameObject.GetComponent<PickableItem>();
                if (pickUpItem == this)
                {
                    inventoryController.InventoryRemove(i, true);
                }
            }
        }
    }

    public void UseStackableItemAsAI()
    {
        if (holderAI != null && inventoryControllerAI != null)
        {
            for (var i = 0; i < inventoryControllerAI.maxSlot; i++)
            {
                var currentSlotItem = inventoryControllerAI?.Inventory[i];
                var pickUpItem = currentSlotItem?.GameObject.GetComponent<PickableItem>();
                if (pickUpItem == this)
                {
                    inventoryControllerAI.InventoryRemove(i, true);
                }
            }
        }
    }
    public virtual void ItemEffect()
    {
        //Override this
    }
}