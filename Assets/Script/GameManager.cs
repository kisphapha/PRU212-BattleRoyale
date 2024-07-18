using Cinemachine;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI playerCountDisplay;
    public TextMeshProUGUI roomDisplay;
    public TextMeshProUGUI startTitleDisplay;
    public TextMeshProUGUI countDownDisplay;
    public int playerCount = 0;
    public int startTime = 30;
    public bool isStarted = false;
    public GameOver gameOverController;
    public AudioClip victoryAudio;
    private PhotonView view;
    // Start is called before the first frame update
    void Start()
    {
        view = GetComponent<PhotonView>();
        gameOverController = GetComponent<GameOver>();
        roomDisplay.text = "Room : " + PhotonNetwork.CurrentRoom.Name;
        if (PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(Countdown());
        }

    }
    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator Countdown()
    {
        while (startTime > 0)
        {
            view.RPC("UpdateCountDownTextRPC", RpcTarget.AllBufferedViaServer, startTime + "s");
            yield return new WaitForSeconds(1f);
            startTime--;
        }
        view.RPC("DisappearDisplay", RpcTarget.AllBufferedViaServer);
        PhotonNetwork.CurrentRoom.IsOpen = false;
    }

    [PunRPC]
    private void UpdateCountDownTextRPC(string value)
    {
        countDownDisplay.text = value;
    }

    [PunRPC]
    private void DisappearDisplay()
    {
        startTitleDisplay.gameObject.SetActive(false);
        countDownDisplay.gameObject.SetActive(false);
        isStarted = true;
    }

    [PunRPC]
    public void PlayerChange()
    {
        GameObject[] prefabInstances = GameObject.FindGameObjectsWithTag("Player");
        playerCount = prefabInstances.Length;
        playerCountDisplay.text = "Players left: " + playerCount.ToString();
        if (playerCount == 1 && isStarted)
        {
            gameOverController.Win(prefabInstances[0]);
            var player = prefabInstances[0].GetComponent<PlayerProps>();
            if (player != null && view.IsMine)
            {
                gameOverController.UpdateKillCount(player.killCount);
                AudioSource.PlayClipAtPoint(victoryAudio, prefabInstances[0].transform.position);
                player.Win();
            }
            var virtualCam = FindObjectOfType<CinemachineVirtualCamera>();
            if (virtualCam != null)
            {
                virtualCam.Follow = prefabInstances[0].transform;
                virtualCam.LookAt = prefabInstances[0].transform;
            }
        }
    }

    public void UpdatePlayerCount()
    {
        view.RPC("PlayerChange", RpcTarget.AllViaServer);
    }

}
