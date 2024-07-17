using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Timeline;

public class DestroyOnCollision : MonoBehaviour
{
    private PhotonView view;
    public GameObject master;
    private void Start()
    {
        view = GetComponent<PhotonView>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
       
        var rb = GetComponent<Rigidbody2D>();
        //if (!PhotonNetwork.IsMasterClient)
        //    return;
        if (other.tag == "Solid")
        {
            if (view != null && view.IsMine)
            {
                PhotonNetwork.Destroy(gameObject); // Destroy the object across the network
            }
        }
        else if (other.tag == "Enermy" || other.tag == "Player")
        {
            PlayerProps playerHealth = other.GetComponent<PlayerProps>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(rb.velocity.magnitude / 2, master);
            }
            PlayerAIProps enemyHealth = other.GetComponent<PlayerAIProps>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(rb.velocity.magnitude / 2, master);
            }
            if (view != null && view.IsMine)
            {
                PhotonNetwork.Destroy(gameObject); // Destroy the object across the network
            }
        }
        else if(other.tag == "Box") 
        {
            BreakBoxes boxHealth = other.GetComponent<BreakBoxes>();
            if (boxHealth != null)
            {
                Debug.Log(rb.velocity.magnitude);
                boxHealth.TakeDamage(rb.velocity.magnitude / 2);
                if (view != null && view.IsMine)
                {
                    PhotonNetwork.Destroy(gameObject); // Destroy the object across the network
                }
            }
        }
        else if (other.tag == "Barrel")
        {
            TNTBarrels tntHealth = other.GetComponent<TNTBarrels>();
            if (tntHealth != null)
            {
                Debug.Log(rb.velocity.magnitude);
                tntHealth.TakeDamage(rb.velocity.magnitude / 2);
                if (view != null && view.IsMine)
                {
                    PhotonNetwork.Destroy(gameObject); // Destroy the object across the network
                }
            }
        }
    }

    //[PunRPC]
    //void DestroyOnCollisionForAll(int viewID)
    //{
    //    PhotonView itemView = PhotonView.Find(viewID);
    //    if (itemView != null)
    //    {
    //        //GameObject item = itemView.gameObject;
    //        Destroy(gameObject);
    //    }
    //}
    //private void RequestDestroy(int viewID)
    //{
    //    view.RPC("DestroyObject", RpcTarget.MasterClient, viewID);
    //}
    //[PunRPC]
    //private void DestroyObject(int viewID)
    //{
    //    Debug.Log("Destroying : " + viewID);
    //    PhotonView targetView = PhotonView.Find(viewID);
    //    if (targetView != null && (targetView.IsMine || PhotonNetwork.IsMasterClient))
    //    {
    //        PhotonNetwork.Destroy(targetView.gameObject);
    //    }
    //}
}
