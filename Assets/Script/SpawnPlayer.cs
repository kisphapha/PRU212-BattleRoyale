using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlayer : MonoBehaviour
{
    public int roomWidth = 182;
    public int roomHeight = 233;
    public GameObject playerPrefab;
    // Start is called before the first frame update
    void Start()
    {
        ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable { { "PlayerName", PhotonNetwork.NickName } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerProperties);

        var position = new Vector3(transform.position.x + Random.Range(0,roomWidth), transform.position.y + Random.Range(0,roomHeight), 0);
        PhotonNetwork.Instantiate(playerPrefab.name, position, Quaternion.identity);
       
    }
}
