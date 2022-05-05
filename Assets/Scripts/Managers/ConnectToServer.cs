using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class ConnectToServer : MonoBehaviourPunCallbacks
{
    [Tooltip("The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created")]
    [SerializeField]
    private byte maxPlayersPerRoom = 2;
    [SerializeField]
    private UnityEvent OnConnectedEvent;
    [SerializeField]
    private UnityEvent OnDisconnectedEvent;

    string gameVersion = "webgl1";

    public void SetPlayerReady()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public void LoadTutorial()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Disconnect();

        }
        SceneManager.LoadScene("Tutorial");
    }


    void Start()
    {
        PhotonNetwork.GameVersion = gameVersion;
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("PUN Basics Tutorial/Launcher:OnJoinRandomFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");

        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
    }

    public override void OnJoinedRoom()
    {
        OnConnectedEvent?.Invoke();
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            PhotonNetwork.LoadLevel("Game");
        }
    }

    #region MonoBehaviourPunCallbacks Callbacks

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarningFormat("PUN Basics Tutorial/Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
        OnDisconnectedEvent?.Invoke();
        Invoke(nameof(SetPlayerReady), 5);
    }

    #endregion

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            PhotonNetwork.LoadLevel("Game");
        }
    }
}
