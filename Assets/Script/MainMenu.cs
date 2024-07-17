using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviourPunCallbacks
{
    public TMP_InputField roomInput;
    public TMP_InputField nameInput;
    public GameManager gameManager;
    public void PlayGame()
    {
        //SceneManager.LoadScene(1);
        PhotonNetwork.JoinRoom(roomInput.text);
    }

    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(roomInput.text);
    }
    public void BackToMainMenu()
    {
        PhotonNetwork.LeaveRoom();
    }
    public override void OnJoinedRoom()
    {
        PhotonNetwork.NickName = nameInput.text;
        PhotonNetwork.LoadLevel("MainScene");
    }
    public override void OnLeftRoom()
    {
        Debug.Log("Left the room. Returning to the main menu.");
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    //public override void OnDisconnected(DisconnectCause cause)
    //{
    //    SceneManager.LoadScene("LoadingScene");
    //}


}
