using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;

public class PlayerLobbyInfo : MonoBehaviourPunCallbacks, IPunObservable
{
    private int playerNumber;

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
        //Load the player info for other clients
        photonView.RPC("updatePlayerInfo", RpcTarget.AllBuffered, PhotonNetwork.LocalPlayer);
    }

    public void unloadPlayer() {
        playerNameText.GetComponent<Text> ().text = null;
        playerReadyText.GetComponent<Text> ().text = null;
    }


    #region Pun RPCs

    [PunRPC]
    void updatePlayerInfo(Player player) {
        playerNameText.GetComponent<Text> ().text = player.NickName;
    }

    #endregion
}
