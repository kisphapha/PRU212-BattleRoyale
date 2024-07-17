using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakBoxes : MonoBehaviour
{
    public float hp = 100;
    public GameObject particles;
    private PhotonView view;
    // Start is called before the first frame update
    void Start()
    {
        view = GetComponent<PhotonView>();
    }

    public void TakeDamage(float amount)
    {
        hp -= amount;
        if (hp <= 0)
        {
            PhotonNetwork.Instantiate(particles.name, transform.position, transform.rotation);         
            //PhotonNetwork.Destroy(gameObject); // Destroy the object across the network
            view.RPC("DestroyObject",RpcTarget.AllBufferedViaServer);

            //if (view.Owner != PhotonNetwork.LocalPlayer)
            //{
            //    view.TransferOwnership(PhotonNetwork.LocalPlayer);
            //    //StartCoroutine(DestroyAfterOwnershipTransfer());
            //}
            //PhotonNetwork.Destroy(gameObject);            
        }
    }
    [PunRPC]
    void DestroyObject()
    {
        //PhotonView itemView = PhotonView.Find(viewID);
        //if (itemView != null)
        //{
            //GameObject item = itemView.gameObject;
            Destroy(gameObject);
        //}
    }
    //private IEnumerator DestroyAfterOwnershipTransfer()
    //{
    //    // Wait for one frame to ensure ownership transfer is complete
    //    yield return null;
    //    PhotonNetwork.Destroy(gameObject);
    //}
}
