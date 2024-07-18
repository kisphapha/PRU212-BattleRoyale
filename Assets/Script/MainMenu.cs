using ExitGames.Client.Photon;
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
    private RoomInfo[] cachedRoomList;

    private void Start()
    {
        // Refresh the list of rooms before attempting to join
        //PhotonNetwork.GetRoomList();
    }
    public void PlayGame()
    {
        CheckRoomAndJoin(roomInput.text);
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
        // Set the player's nickname
        PhotonNetwork.NickName = nameInput.text;
        // Load the main game scene
        PhotonNetwork.LoadLevel("MainScene");
    }
    private void CheckRoomAndJoin(string roomName)
    {
        //Debug.Log(cachedRoomList[0]);
        if (cachedRoomList != null)
        {
            foreach (RoomInfo room in cachedRoomList)
            {
                //Debug.Log("Comparing " + room.Name + " to " + roomName);
                if (room.Name == roomName)
                {
                    if (!room.IsOpen)
                    {
                        Debug.Log("Room has started");
                        return;
                    }
                    PhotonNetwork.JoinRoom(roomName);
                    return;
                }
            }
        }        
        Debug.Log("Cannot join room");
    }


    // Callback to get the updated list of rooms
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("Rooms updated");
        cachedRoomList = roomList.ToArray();
    }

    public override void OnLeftRoom()
    {
        Debug.Log("Left the room. Returning to the main menu.");
        //PhotonNetwork.LeaveLobby();
        SceneManager.LoadScene("LoadingScene");
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
