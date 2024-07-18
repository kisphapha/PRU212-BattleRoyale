using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class PickupController : MonoBehaviour
{
    private PlayerProps master;

    // List to store available weapons
    private List<GameObject> availableWeapons = new List<GameObject>();
    private InventoryController inventoryController;
    private int currentWeaponIndex = 0; // Index of the currently equipped weapon
    private PhotonView view;
    private GameManager gameManager;

    void Start()
    {
        inventoryController = GetComponent<InventoryController>();
        master = GetComponent<PlayerProps>();
        view = GetComponent<PhotonView>();
        gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        if (view.IsMine && gameManager.isStarted)
        {
            CarryTheItem();

            // Check for input to drop or switch the weapon
            if (Input.GetKeyDown(KeyCode.G))
            {
                DropWeapon();
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                SwitchWeapon();
                //PickUpHealingItem();
            }
        }
       
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (view != null && view.IsMine)
        {
            // Check if the colliding object is a weapon
            if (other.CompareTag("PickUpItems"))
            {
                // Add the weapon to the list of available weapons
                availableWeapons.Add(other.gameObject);
            }
        }     
       
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (view != null && view.IsMine)
        {
            // Check if the exiting object is a weapon
            if (other.CompareTag("PickUpItems"))
            {
                // Remove the weapon from the list of available weapons
                availableWeapons.Remove(other.gameObject);
            }
        }
    }


    public void CarryTheItem()
    {
        if (view.IsMine)
        {
            var item = inventoryController.Inventory[inventoryController.currentSlot - 1];
            if (item != null && item.GameObject != null)
            {
                var gun = item.GameObject.GetComponent<GunEntity>();
                var deviationAngle = 0;
                if (gun != null)
                {
                    deviationAngle = gun.spriteAngle;
                }
                float itemSpriteLength = item.GameObject.GetComponent<SpriteRenderer>().sprite.bounds.size.x
                    * item.GameObject.transform.lossyScale.x;
                float angle = master.angle;
                Quaternion itemRotation = Quaternion.Euler(0f, 0f, angle - deviationAngle - 90);
                Quaternion masterRotation = Quaternion.Euler(0f, 0f, angle - 90);
                Vector3 targetPosition = transform.position + (masterRotation * Vector3.right * itemSpriteLength / 2);
                //item.GameObject.transform.position = targetPosition;
                //item.GameObject.transform.rotation = itemRotation;
                view.RPC("SyncItemTransform", RpcTarget.All, item.GameObject.GetPhotonView().ViewID, targetPosition, itemRotation);
            }
        }
    }

    public void SwitchWeapon()
    {
        // Drop the current weapon
        //DropWeapon();
        if (inventoryController != null)
        {
            // Check if there are available weapons
            if (availableWeapons.Count > 0)
            {
                // Move to the next weapon in the list
                currentWeaponIndex = (currentWeaponIndex + 1) % availableWeapons.Count;
                var weapon = availableWeapons[currentWeaponIndex];
                // Equip the new weapon
                var result = inventoryController.InventoryAdd(weapon);
                if (result == 1)
                {
                    EquipWeapon(weapon);
                } else if (result == 2)
                {
                    DestroyItem(weapon);
                }
            }
        }
    }

    public void EquipWeapon(GameObject weapon)
    {
        if (view.IsMine)
        {
            // Transfer ownership to the current client
            PhotonView weaponView = weapon.GetComponent<PhotonView>();
            if (weaponView != null && !weaponView.IsMine)
            {
                weaponView.TransferOwnership(PhotonNetwork.LocalPlayer);
            }

            // Call the RPC method to equip the weapon
            view.RPC("EquipWeaponRPC", RpcTarget.AllBuffered, weaponView.ViewID);
        }
    }
    public void DestroyItem(GameObject gameObject)
    {
        if (view.IsMine)
        {
            view.RPC("DestroyItemRPC", RpcTarget.AllBuffered, gameObject.GetPhotonView().ViewID);
        }
    }
    public void DropStackedItem(GameObject gameObject, GameObject sample)
    {
        if (view.IsMine)
        {
            view.RPC("DropStackedItemRPC", RpcTarget.AllBuffered, gameObject.name, transform.position, transform.rotation, sample.transform.lossyScale, sample.name);
        }
    }

    [PunRPC]
    private void DropStackedItemRPC(string itemName, Vector3 position, Quaternion rotation, Vector3 scale, string sampleName)
    {
        var item = PhotonNetwork.Instantiate(itemName, position, rotation);
        item.transform.localScale = scale;
        item.name = sampleName;
    }
    public void DropWeapon()
    {
        if (view.IsMine)
        {
            var item = inventoryController.Inventory[inventoryController.currentSlot - 1];
            if (item != null)
            {
                if (item.Amount > 1)
                {
                    DropStackedItem(item.GameObject, master.holdingItem);
                    inventoryController.InventoryRemove(inventoryController.currentSlot - 1);
                }
                else
                {
                    inventoryController.InventoryRemove(inventoryController.currentSlot - 1);
                    item.GameObject.SetActive(true);
                    item.GameObject.transform.SetParent(null); // Remove the player character as the parent of the picked object
                    item.GameObject.GetComponent<Collider2D>().enabled = true;
                    master.isHeldingGun = false;

                    var currentItem = master.holdingItem?.GetComponent<PickableItem>();
                    if (currentItem != null)
                    {
                        currentItem.holder = null;
                        currentItem.inventoryController = null;
                    }

                    var weapon = master.holdingItem?.GetComponent<GunEntity>();
                    if (weapon != null)
                    {
                        weapon.ResetAmmoDisplay();
                    }

                    master.holdingItem = null;

                    // Sync the drop across the network
                    view.RPC("DropWeaponRPC", RpcTarget.AllBuffered, item.GameObject.GetPhotonView().ViewID);
                }
            }
        }
    }

    [PunRPC]
    private void DropWeaponRPC(int itemViewID)
    {
        PhotonView itemView = PhotonView.Find(itemViewID);
        if (itemView != null)
        {
            GameObject item = itemView.gameObject;
            item.SetActive(true);
            item.transform.SetParent(null);
            item.GetComponent<Collider2D>().enabled = true;
        }
    }

    [PunRPC]
    void SyncItemTransform(int itemViewId, Vector3 targetPosition, Quaternion itemRotation)
    {
        PhotonView itemView = PhotonView.Find(itemViewId);
        if (itemView != null)
        {
            GameObject item = itemView.gameObject;
            item.transform.position = targetPosition;
            item.transform.rotation = itemRotation;
        }
    }

    [PunRPC]
    private void EquipWeaponRPC(int weaponViewID)
    {
        // Find the weapon by its PhotonView ID
        PhotonView weaponView = PhotonView.Find(weaponViewID);
        if (weaponView != null)
        {
            GameObject weapon = weaponView.gameObject;
            weapon.transform.SetParent(transform); // Make the player character the parent of the picked object
            weapon.GetComponent<Collider2D>().enabled = false;
        }
        else
        {
            Debug.LogError("Failed to find weapon with PhotonView ID: " + weaponViewID);
        }
    }
    [PunRPC]
    private void DestroyItemRPC(int viewId)
    {
        PhotonView itemView = PhotonView.Find(viewId);
        if (itemView != null)
        {
            GameObject item = itemView.gameObject;
            Destroy(item);
        }
    }
}