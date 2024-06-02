using UnityEngine;

public class PickableItem : MonoBehaviour
{
    public PlayerProps holder;
    public InventoryController inventoryController;
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
                Debug.Log(pickUpItem.name);
                if (timer <= 0f && pickUpItem == this)
                {
                    UseStackableItem();
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
            Debug.Log("Using item....");
            for (var i = 0; i < inventoryController.maxSlot; i++)
            {
                var currentSlotItem = inventoryController?.Inventory[i];
                var pickUpItem = currentSlotItem?.GameObject.GetComponent<PickableItem>();
                if (pickUpItem != null)
                {
                    inventoryController.InventoryRemove(i);
                }
            }
        }    
    }
    public virtual void ItemEffect()
    {
        //Override this
    }
}