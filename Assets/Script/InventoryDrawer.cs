using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class InventoryDrawer : MonoBehaviour
{
    // Start is called before the first frame update
    public Image inventorySlot1;
    public Image inventorySlot2;
    public Image inventorySlot3;
    public Image inventorySlotFrame1;
    public Image inventorySlotFrame2;
    public Image inventorySlotFrame3;
    private InventoryController inventoryController;

    private void Start()
    {
        SetImageVisibility(inventorySlot1,false);
        SetImageVisibility(inventorySlot2,false);
        SetImageVisibility(inventorySlot3,false);
        inventoryController = GetComponent<InventoryController>();
        UpdateInventoryFrame();
    }
    public void SetImageVisibility(Image image, bool visible)
    {
        if ( image != null)
        {
            if (visible)
            {
                // Set the Image to be visible
                image.color = new Color(1, 1, 1, 1);
            }
            else
            {
                // Set the Image to be invisible
                image.color = new Color(1, 1, 1, 0);
            }
        }     
    }
    public void UpdateInventoryFrame()
    {
        var sprite1 = (Sprite)AssetDatabase.LoadAssetAtPath("Assets/Sprites/spr_inventory_slot.png", typeof(Sprite)); 
        var sprite2 = (Sprite)AssetDatabase.LoadAssetAtPath("Assets/Sprites/spr_inventory_slot_2.png", typeof(Sprite));

        //Debug.Log(Resources.Load<Sprite>("spr_inventory_slot").name);
        inventorySlotFrame1.sprite = sprite1;
        inventorySlotFrame2.sprite = sprite1;
        inventorySlotFrame3.sprite = sprite1;
        switch (inventoryController.currentSlot)
        {
            case 1 :
                inventorySlotFrame1.sprite = sprite2;
                break;
            case 2:
                inventorySlotFrame2.sprite = sprite2;
                break;
            case 3:
                inventorySlotFrame3.sprite = sprite2;
                break;
        }
    }
    public void UpdateInventoryDisplay()
    {
        // Update the Sprite of each Image object based on the Inventory in PlayerProps
        int slot = 0;
        foreach (var item in inventoryController.Inventory)
        {
            slot++;
            if (item != null)
            {
                switch (slot)
                {
                    case 1:
                        inventorySlot1.sprite = item.GetComponent<SpriteRenderer>().sprite;
                        SetImageVisibility(inventorySlot1, true);
                        break;
                    case 2:
                        inventorySlot2.sprite = item.GetComponent<SpriteRenderer>().sprite;
                        SetImageVisibility(inventorySlot2, true);
                        break;
                    case 3:
                        inventorySlot3.sprite = item.GetComponent<SpriteRenderer>().sprite;
                        SetImageVisibility(inventorySlot3, true);
                        break;
                }
            } else
            {
                switch (slot)
                {
                    case 1:
                        inventorySlot1.sprite = null;
                        SetImageVisibility(inventorySlot1, false);
                        break;
                    case 2:
                        inventorySlot2.sprite = null;
                        SetImageVisibility(inventorySlot2, false);
                        break;
                    case 3:
                        inventorySlot3.sprite = null;
                        SetImageVisibility(inventorySlot3, false);
                        break;
                }
            }
        }
        
    }

}
