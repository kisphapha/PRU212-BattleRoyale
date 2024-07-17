using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPickUp : MonoBehaviour
{
    public GameObject pickUp;
    public CustomTrigger triggerPickUp;
    //public CustomTrigger triggerPlayer;
    private GameObject pickedObject; // Reference to the currently picked object
    private PlayerAIProps master;
    private AIBehavior chase;
    // List to store available weapons
    private AIInventory inventoryController;
    private PhotonView view;

    public float detectionRadius = 1f;
    public float speed = 1f;
    public int count = 3;

    void Start()
    {
        view = GetComponent<PhotonView>();
        inventoryController = GetComponent<AIInventory>();
        master = GetComponent<PlayerAIProps>();
        chase = GetComponent<AIBehavior>();
        //chase.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        CarryTheItem();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the colliding object is a weapon
        if (other.CompareTag("PickUpItems"))
        {
            if (inventoryController != null)
            {
                // Move to the next weapon in the list
                var weapon = other.gameObject;
                // Equip the new weapon
                var gun = weapon.GetComponent<GunEntity>();
                if (gun != null && gun.currentAmmo == 0)
                {
                    return;
                }
                var result = inventoryController.InventoryAdd(weapon);
                if (result == 1)
                {
                    EquipWeapon(weapon);
                }
                else if (result == 2)
                {
                    view.RPC("DestroyItemRPC", RpcTarget.AllBuffered, weapon.GetPhotonView().ViewID);
                }
            }
            //Debug.Log("should pick up");

            //EquipWeapon(other.gameObject);
            //if (count <= 0)
            //{
            //    active = true;
            //}
            //other.gameObject.tag = "Untagged";
            //this.enabled = false;
        }
    }


    public void CarryTheItem()
    {
        var item = inventoryController.Inventory[inventoryController.currentSlot - 1];
        if (item != null)
        {
            var gun = item.GameObject.GetComponent<GunEntity>();
            var deviationAngle = 0;
            if (gun != null)
            {
                deviationAngle = gun.spriteAngle;
            }
            float itemSpriteLength = item.GameObject.GetComponent<SpriteRenderer>().sprite.bounds.size.x
                * item.GameObject.transform.lossyScale.x;

            Vector3 direction = master.transform.position - transform.position;
            direction.z = master.transform.position.z;
            float angle = chase.angle;
            Quaternion itemRotation = Quaternion.Euler(0f, 0f, angle - deviationAngle);
            Quaternion masterRotation = Quaternion.Euler(Vector3.forward * angle);
            Vector3 targetPosition = transform.position + (masterRotation * Vector3.right * itemSpriteLength/2);
            view.RPC("SyncItemTransform", RpcTarget.All, item.GameObject.GetPhotonView().ViewID, targetPosition, itemRotation);
        }
    }
    public void EquipWeapon(GameObject weapon)
    {
        PhotonView weaponView = weapon.GetComponent<PhotonView>();
        if (weaponView != null && !weaponView.IsMine)
        {
            weaponView.TransferOwnership(PhotonNetwork.LocalPlayer);
        }

        // Call the RPC method to equip the weapon
        view.RPC("EquipWeaponRPC", RpcTarget.AllBuffered, weaponView.ViewID);
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
