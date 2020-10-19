using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;

public class PlayerLobbyInfo : MonoBehaviourPunCallbacks, IPunObservable
{
    private Player player;
    private bool isReady;

    [SerializeField]
    private GameObject playerNameText;
    [SerializeField]
    private GameObject playerReadyText;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    #region IPunObservable implementation

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {

    }

    #endregion


    public void loadPlayer() {
        //Load the player info for all clients
        photonView.RPC("updatePlayerInfo", RpcTarget.AllBuffered, PhotonNetwork.LocalPlayer);
    }

    public void unloadPlayer() {
        playerNameText.GetComponent<Text> ().text = null;
        playerReadyText.GetComponent<Text> ().text = null;
    }

    public void toggleReadyStatus() {
        isReady = !isReady; 
        photonView.RPC("updatePlayerReadyStatus", RpcTarget.AllBuffered, isReady);
    }

    public bool hasPlayer() {
        if (player != null) {
            return true;
        }
        return false;
    }

    public bool getIsReady() {
        return isReady;
    }

    public void setReady(bool _ready) {
        isReady = _ready;
    }


    #region Pun RPCs

    [PunRPC]
    void updatePlayerInfo(Player _player) {
        player = _player;
        playerNameText.GetComponent<Text> ().text = _player.NickName;

    }

    [PunRPC]
    void updatePlayerReadyStatus(bool ready) {
        isReady = ready;
        if (ready) {
            playerReadyText.GetComponent<Text> ().enabled = true;
        } else {
            playerReadyText.GetComponent<Text> ().enabled = false;
        }
    }

    #endregion
}
