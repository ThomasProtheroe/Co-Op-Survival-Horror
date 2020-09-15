using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{    
    
    #region Private Serializable References

    [SerializeField]
    private GameObject playerPrefab;
    [SerializeField]
    private List<GameObject> playerSpawns;

    #endregion


    #region Private references

    private List<Player> playerList;

    #endregion

    

    void Start() {
        //Get player spawn locations
        playerSpawns = new List<GameObject> (GameObject.FindGameObjectsWithTag("PlayerSpawn"));
        playerList = new List<Player> (PhotonNetwork.PlayerList);

        if (playerPrefab == null) {
            Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'",this);
        } else {
            if (SurvivorController.localPlayerInstance == null) {
                Debug.LogFormat("We are Instantiating LocalPlayer from {0}", Application.loadedLevelName);
                // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
                //Select a spawn location based on the players ActorNumber
                Vector3 spawnLocation = playerSpawns[playerList.IndexOf(PhotonNetwork.LocalPlayer)].transform.position;
                PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(spawnLocation.x, spawnLocation.y, 0.0f), Quaternion.identity, 0);
            } else {
                Debug.LogFormat("Do not instantiate player for scene load {0}", SceneManagerHelper.ActiveSceneName);
            }
        }
    }


    #region Public Methods


    #endregion


    #region Private Methods


    #endregion
}