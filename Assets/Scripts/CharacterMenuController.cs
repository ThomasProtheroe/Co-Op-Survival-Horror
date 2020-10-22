using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class CharacterMenuController : MonoBehaviour
{
    [SerializeField]
    private GameObject createCharacterPanel;
    [SerializeField]
    private GameObject characterNameField;

    [SerializeField]
    private GameObject characterClassField;

    private string characterName;
    
    public void loadCharacterList() {
        
    }
    
    public void createNewCharacter() {
        Dropdown classDropdown = characterClassField.GetComponent<Dropdown> ();
        string characterClass = classDropdown.options[classDropdown.value].text;
        string characterName = characterNameField.GetComponent<InputField> ().text;

        CharacterManager.createNewCharacter(characterName, characterClass);
    }

    public void showCreateCharacterPanel() {
        createCharacterPanel.SetActive(true);
    }

    public void hideCreateCharacterPanel() {
        createCharacterPanel.SetActive(false);
    }
}
