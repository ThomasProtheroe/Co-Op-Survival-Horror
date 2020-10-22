using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class CharacterInfo : MonoBehaviour
{
    [SerializeField]
    private GameObject portrait;
    [SerializeField]
    private GameObject nameText;
    [SerializeField]
    private GameObject classText;
    [SerializeField]
    private GameObject levelText;

    private Character character;

    public void loadCharacter(Character inCharacter) {
        character = inCharacter;

        //Update UI elements
        //portrait
        nameText.GetComponent<Text> ().text = character.characterName;
        classText.GetComponent<Text> ().text = character.className;
        levelText.GetComponent<Text> ().text = character.level.ToString();
    }
}
