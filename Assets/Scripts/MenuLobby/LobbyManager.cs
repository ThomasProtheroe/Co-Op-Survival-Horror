using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;

public class LobbyManager : MonoBehaviourPunCallbacks, IPunObservable
{
    #region Private Fields


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
    [SerializeField]
    private GameObject readyRequestText;
    [SerializeField]
    private List<PlayerLobbyInfo> lobbySlots;
    
    #endregion


    #region Private References

    private PlayerLobbyInfo localSlot;
    private List<Player> playerList;
    private ExitGames.Client.Photon.Hashtable hash;

    #endregion


    // Start is called before the first frame update
    void Start()
    {
        //Initialize references/fields
        playerList = new List<Player> (PhotonNetwork.PlayerList);
        localSlot = lobbySlots[playerList.IndexOf(PhotonNetwork.LocalPlayer)];

        localSlot.loadPlayer();

        //Show the lobby title
        lobbyTitle.GetComponent<Text> ().text = PhotonNetwork.CurrentRoom.Name;

        //Show the right Join/Start button for server/client
        if (PhotonNetwork.IsMasterClient) {
            startButton.SetActive(true);
        } else {
            readyButton.SetActive(true);
        }

        Character currentCharacter = CharacterManager.getCurrentCharacter();
        currentCharacter.addWeapon("Handgun");
        currentCharacter.addWeapon("Knife");
        currentCharacter.addArmor("Kevlar Vest");
    }


    #region IPunObservable implementation

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {

    }

    #endregion


    public void readyClick() {
        if (!localSlot.getIsReady()) {
            readyButton.GetComponent<Image> ().color = new Color32(126,255,126,100);
        } else {
            readyButton.GetComponent<Image> ().color = new Color32(255,255,255,100);
        }

        localSlot.toggleReadyStatus();
    }

    public void startClick() {
        if (!PhotonNetwork.IsMasterClient) {
            //Only master client should be able to start the game
            return;
        }

        localSlot.setReady(true);

        //Check Ready status of all players
        foreach (PlayerLobbyInfo slot in lobbySlots) {
            if (slot.hasPlayer() && !slot.getIsReady()) {
                photonView.RPC("displayReadyRequest", RpcTarget.AllBuffered);
                return;
            }
        }

        LoadRandomLevel();
    }

    public void leaveClick() {
        LeaveRoom();
    }

    public void sendLobbyMessage(string message) {

    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    private void LoadRandomLevel()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
        }
        PhotonNetwork.LoadLevel("RandomLevel");
    }


    #region Pun RPCs

    [PunRPC]
    void displayReadyRequest() {
        readyRequestText.GetComponent<Text> ().enabled = true;
    }

    #endregion


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