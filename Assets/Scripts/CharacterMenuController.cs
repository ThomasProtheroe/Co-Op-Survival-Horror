using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class CharacterMenuController : MonoBehaviour
{
    [SerializeField]
    private GameObject createCharacterPanel;
    [SerializeField]
    private GameObject characterListPanel;

    [SerializeField]
    private List<CharacterInfo> characterInfos;
    [SerializeField]
    private GameObject characterInfoPrefab;

    [SerializeField]
    private GameObject characterNameField;
    [SerializeField]
    private GameObject characterClassField;

    private string characterName;
    
    public void loadCharacterList() {
        foreach(Transform child in characterListPanel.transform) {
            Destroy(child.gameObject);
        }
        foreach(Character character in CharacterManager.getCharacters()) {
            GameObject infoPanel = Instantiate(characterInfoPrefab, characterListPanel.transform);
            CharacterInfo info = infoPanel.GetComponent<CharacterInfo> ();
            info.loadCharacter(character);
            info.updateColor();
        }
    }
    
    public void createNewCharacter() {
        Dropdown classDropdown = characterClassField.GetComponent<Dropdown> ();
        string characterClass = classDropdown.options[classDropdown.value].text;
        string characterName = characterNameField.GetComponent<InputField> ().text;

        CharacterManager.createNewCharacter(characterName, characterClass);

        loadCharacterList();
    }

    public void showCreateCharacterPanel() {
        createCharacterPanel.SetActive(true);
    }

    public void hideCreateCharacterPanel() {
        createCharacterPanel.SetActive(false);
    }
}
