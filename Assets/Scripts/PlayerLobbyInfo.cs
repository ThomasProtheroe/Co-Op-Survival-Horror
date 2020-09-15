using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;

public class PlayerLobbyInfo : MonoBehaviour
{
    private Player player;

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

    public loadPlayer(Player inPlayer) {
        player = inPlayer;
        playerNameText.GetComponent<Text> ().text = player.NickName;
    }
}
