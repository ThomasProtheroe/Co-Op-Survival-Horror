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
                // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
                //Select a spawn location based on the players number (index in playerList)
                Vector3 spawnLocation = playerSpawns[playerList.IndexOf(PhotonNetwork.LocalPlayer)].transform.position;
                GameObject newSurvivor = PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(spawnLocation.x, spawnLocation.y, 0.0f), Quaternion.identity, 0);
                
                //Set players gear from custom player properties
                Dictionary<string,float> weaponDict1 = Armory.getWeapon((string)PhotonNetwork.LocalPlayer.CustomProperties["weapon1"]);
                Dictionary<string,float> weaponDict2 = Armory.getWeapon((string)PhotonNetwork.LocalPlayer.CustomProperties["weapon2"]);
                newSurvivor.GetComponent<SurvivorController> ().addWeapon(weaponDict1);
                newSurvivor.GetComponent<SurvivorController> ().addWeapon(weaponDict2);
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