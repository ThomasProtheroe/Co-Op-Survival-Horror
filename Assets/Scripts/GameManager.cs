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
    private List<GameObject> playerPrefabs;
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
        Character currentCharacter = CharacterManager.getCurrentCharacter();
        GameObject classPrefab = playerPrefabs[currentCharacter.classType];

        if (classPrefab == null) {
            Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'",this);
        } else {
            if (SurvivorController.localPlayerInstance == null) {
                // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
                //Select a spawn location based on the players number (index in playerList)
                Vector3 spawnLocation = playerSpawns[playerList.IndexOf(PhotonNetwork.LocalPlayer)].transform.position;
                GameObject newSurvivor = PhotonNetwork.Instantiate(classPrefab.name, new Vector3(spawnLocation.x, spawnLocation.y, 0.0f), Quaternion.identity, 0);
                
                //Set players gear from character data
                foreach(string weapon in CharacterManager.getCurrentCharacter().weapons) {
                    newSurvivor.GetComponent<SurvivorController> ().addWeapon(weapon);
                }
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