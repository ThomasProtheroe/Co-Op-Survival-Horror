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
    public GameObject selectButton;

    private Character character;

    void Start() {
        selectButton.GetComponent<Button> ().onClick.AddListener(makeActiveCharacter);
    }

    public void loadCharacter(Character inCharacter) {
        character = inCharacter;

        //Update UI elements
        //TODO - portrait
        nameText.GetComponent<Text> ().text = character.characterName;
        classText.GetComponent<Text> ().text = character.className;
        levelText.GetComponent<Text> ().text = character.level.ToString();
    }

    public void updateColor() {
        if(character == CharacterManager.getCurrentCharacter()) {
            selectButton.GetComponent<Image> ().color = Color.green;
        } else {
            selectButton.GetComponent<Image> ().color = Color.white;
        }
    }

    private void makeActiveCharacter() {
        Debug.Log("Active Character: " + character.characterName);
        CharacterManager.setCurrentCharacter(character);
        updateColor();
    }
}
