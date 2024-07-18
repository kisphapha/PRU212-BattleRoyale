using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class InventoryDrawer : MonoBehaviour
{
    // Start is called before the first frame update
    private Image inventorySlot1;
    private Image inventorySlot2;
    private Image inventorySlot3;
    private Image inventorySlotFrame1;
    private Image inventorySlotFrame2;
    private Image inventorySlotFrame3;
    private Text inventorySlotAmount1;
    private Text inventorySlotAmount2;
    private Text inventorySlotAmount3;
    private InventoryController inventoryController;
    private PhotonView view;
    private void Start()
    {
        view = GetComponent<PhotonView>();
        if (view.IsMine)
        {
            inventorySlot1 = GameObject.Find("Slot1").GetComponent<Image>();
            inventorySlot2 = GameObject.Find("Slot2").GetComponent<Image>();
            inventorySlot3 = GameObject.Find("Slot3").GetComponent<Image>();
            inventorySlotAmount1 = GameObject.Find("Slot1_Amount").GetComponent<Text>();
            inventorySlotAmount2 = GameObject.Find("Slot2_Amount").GetComponent<Text>();
            inventorySlotAmount3 = GameObject.Find("Slot3_Amount").GetComponent<Text>();
            inventorySlotFrame1 = GameObject.Find("Slot1_Frame").GetComponent<Image>();
            inventorySlotFrame2 = GameObject.Find("Slot2_Frame").GetComponent<Image>();
            inventorySlotFrame3 = GameObject.Find("Slot3_Frame").GetComponent<Image>();

            SetImageVisibility(inventorySlot1, false);
            SetImageVisibility(inventorySlot2, false);
            SetImageVisibility(inventorySlot3, false);
            inventorySlotAmount1.text = "";
            inventorySlotAmount3.text = "";
            inventorySlotAmount2.text = "";
            inventoryController = GetComponent<InventoryController>();
            UpdateInventoryFrame();
        }
           
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
        var sprite1 = Resources.Load<Sprite>("spr_inventory_slot"); 
        var sprite2 = Resources.Load<Sprite>("spr_inventory_slot_2");
        if (inventorySlotFrame1 != null && inventorySlotFrame2 != null && inventorySlotFrame3 != null)
        {

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
                        inventorySlot1.sprite = item.GameObject.GetComponent<SpriteRenderer>().sprite;
                        inventorySlotAmount1.text = item.Amount.ToString();
                        SetImageVisibility(inventorySlot1, true);
                        break;
                    case 2:
                        inventorySlot2.sprite = item.GameObject.GetComponent<SpriteRenderer>().sprite;
                        inventorySlotAmount2.text = item.Amount.ToString();
                        SetImageVisibility(inventorySlot2, true);
                        break;
                    case 3:
                        inventorySlot3.sprite = item.GameObject.GetComponent<SpriteRenderer>().sprite;
                        inventorySlotAmount3.text = item.Amount.ToString();
                        SetImageVisibility(inventorySlot3, true);
                        break;
                }
            } else
            {
                switch (slot)
                {
                    case 1:
                        inventorySlot1.sprite = null;
                        inventorySlotAmount1.text = "";
                        SetImageVisibility(inventorySlot1, false);
                        break;
                    case 2:
                        inventorySlot2.sprite = null;
                        inventorySlotAmount2.text = "";
                        SetImageVisibility(inventorySlot2, false);
                        break;
                    case 3:
                        inventorySlot3.sprite = null;
                        inventorySlotAmount3.text = "";
                        SetImageVisibility(inventorySlot3, false);
                        break;
                }
            }
        }
        
    }

}
