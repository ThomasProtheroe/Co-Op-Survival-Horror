using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    #region Private Fields

    private bool isReady;

    #endregion


    #region Private Serializable References
    
    [SerializeField]
    private GameObject playerInfoPrefab;
    [SerializeField]
    private GameObject readyButton;
    [SerializeField]
    private GameObject startButton;
    [SerializeField]
    private GameObject lobbyTitle;
    
    #endregion


    #region Private References

    private List<Player> playerList;
    private List<PlayerLobbyInfo> lobbySlots;

    #endregion


    // Start is called before the first frame update
    void Start()
    {
        //Initialize references/fields
        playerList = new List<Player> (PhotonNetwork.PlayerList);
        lobbySlots = new List<PlayerLobbyInfo> ();

        //Load player names
        foreach(GameObject slot in GameObject.FindGameObjectsWithTag("PlayerLobbyInfo")) {
            lobbySlots.Add(slot.GetComponent<PlayerLobbyInfo> ());
        }

        lobbySlots[playerList.IndexOf(PhotonNetwork.LocalPlayer)].loadPlayer();

        //Show the lobby title
        lobbyTitle.GetComponent<Text> ().text = PhotonNetwork.CurrentRoom.Name;

        //Show the right Join/Start button for server/client
        if (PhotonNetwork.IsMasterClient) {
            startButton.SetActive(true);
        } else {
            readyButton.SetActive(true);
        }
    }

    public void readyClick() {
        if (!isReady) {
            isReady = true;
            readyButton.GetComponent<Image> ().color = new Color32(126,255,126,100);
        } else {
            isReady = false;
            readyButton.GetComponent<Image> ().color = new Color32(255,255,255,100);
        }

        foreach (Player player in playerList) {
            Debug.Log("Player " + playerList.IndexOf(player) + " - " + player.NickName);
        }
    }

     public void startClick() {
        if (!isReady) {
            isReady = true;
            readyButton.GetComponent<Image> ().color = new Color32(126,255,126,100);
        } else {
            isReady = false;
            readyButton.GetComponent<Image> ().color = new Color32(255,255,255,100);
        }
    }

    public void sendLobbyMessage(string message) {

    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public void LoadRandomLevel()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
        }
        Debug.LogFormat("PhotonNetwork : Loading Level : {0}", PhotonNetwork.CurrentRoom.PlayerCount);
        PhotonNetwork.LoadLevel("RandomLevel");
    }


    #region Photon Callbacks

    /// <summary>
    /// Called when the local player left the room. We need to load the menu scene.
    /// </summary>
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }

    #endregion
}