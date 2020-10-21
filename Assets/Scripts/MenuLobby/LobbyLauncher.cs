using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;

public class LobbyLauncher : MonoBehaviourPunCallbacks
{

    #region Private Serializable Fields

    [Tooltip("The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created")]
    [SerializeField]
    private byte maxPlayersPerRoom = 4;
    
    #endregion


    #region Private Serializable References

    [SerializeField]
    private GameObject creatingLobbyText;
    [SerializeField]
    private GameObject joiningLobbyText;

    [SerializeField]
    private GameObject hostedLobbyField;
    [SerializeField]
    private GameObject joiningLobbyField;

    #endregion


    #region private fields


    /// <summary>
    /// This client's version number. Users are separated from each other by gameVersion (which allows you to make breaking changes).
    /// </summary>
    string gameVersion = "1";
    private bool hostGame;
    private bool joinGame;


    #endregion


    #region MonoBehaviour CallBacks


    /// <summary>
    /// MonoBehaviour method called on GameObject by Unity during early initialization phase.
    /// </summary>
    void Awake()
    {
        // #Critical
        // this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public void hostNewLobby() {
        hostGame = true;
        joinGame = false;
        creatingLobbyText.SetActive(true);
        Connect();
    }

    public void joinLobby() {
        hostGame = false;
        joinGame = true;
        joiningLobbyText.SetActive(true);
        Connect();
    }

    private void Connect()
    {
        // we check if we are connected or not, we join if we are , else we initiate the connection to the server.
        if (!PhotonNetwork.IsConnected) {
           // #Critical, we must first and foremost connect to Photon Online Server.
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = gameVersion;
        } else {
            hostOrJoinLobby();
        }
    }

    private void hostOrJoinLobby() {
        if(hostGame) {
            hostGame = false;
            CreateRoom();
        } else if (joinGame) {
            JoinRoom();
        }
    }

    private void CreateRoom() {
        PhotonNetwork.CreateRoom(hostedLobbyField.GetComponent<InputField> ().text, new RoomOptions{ MaxPlayers = maxPlayersPerRoom });
    }

    private void JoinRoom() {
        PhotonNetwork.JoinRoom(joiningLobbyField.GetComponent<InputField> ().text);
    }

    #endregion

    #region MonoBehaviourPunCallbacks Callbacks

    public override void OnConnectedToMaster()
    {
        Debug.Log("PUN Basics Tutorial/Launcher: OnConnectedToMaster() was called by PUN");
        hostOrJoinLobby();
    }


    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarningFormat("PUN Basics Tutorial/Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to join a room.");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("PUN Basics Tutorial/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.");
        SceneManager.LoadScene(1);
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("PUN Basics Tutorial/Launcher: OnCreatedRoom() called by PUN.");
        SceneManager.LoadScene(1);
    }

    #endregion
}
