using Photon.Pun;
using System.Collections;
using UnityEngine;

public class DestroyAfterATime : MonoBehaviour
{
    public float destroyDelay = 3f; // Delay in seconds before destroying the object
    private PhotonView view;
    [SerializeField]
    private bool isLocked = false;
    private void Start()
    {
        view = GetComponent<PhotonView>();
        if (!isLocked)
        {
            Unlock();
        }
    }

    public void Unlock()
    {
        view = GetComponent<PhotonView>();
        view.RPC("DestroyForAllAfterATime", RpcTarget.AllBufferedViaServer);
    }

    [PunRPC]
    private void DestroyForAllAfterATime()
    {
        Destroy(gameObject,destroyDelay);
    }
}
