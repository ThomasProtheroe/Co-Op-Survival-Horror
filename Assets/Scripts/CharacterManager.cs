using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class CharacterManager
 {
    private static string savePath;
    private static List<Character> allCharacters;
    private static Character currentCharacter;
    static CharacterManager() {
        savePath = Application.persistentDataPath + "/savedata/characters/";
    }
        
    /**
    * Saves the save data to the disk
    */
    public static void saveCharacterToDisk(Character character) {
        string path = savePath + character.characterName + ".sav";
        BinaryFormatter bf = new BinaryFormatter();
        createSaveDirectory();
        FileStream file = File.Create(path);
        bf.Serialize(file, character);
        file.Close();
    }

    /**
    * Loads the save data from the disk
    */
    public static Character loadCharacterFromDisk(string characterPath) {
        if(File.Exists(characterPath))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(characterPath, FileMode.Open);
            Character character = (Character)bf.Deserialize(file);
            file.Close();

            return character;
        }
        return null;
    }

    public static void loadAllCharacters() {
        createSaveDirectory();
        allCharacters = new List<Character> ();
        foreach (string file in System.IO.Directory.GetFiles(savePath)) {
            Character temp = loadCharacterFromDisk(file);
            if (temp != null) {
                allCharacters.Add(temp);
            }
        }
    }

    private static void createSaveDirectory() {
        string path = savePath + "default.sav";
        DirectoryInfo di = Directory.CreateDirectory(path);
    }

    public static void createNewCharacter(string newCharacterName, string newCharacterClass) {
        createSaveDirectory();
        Character newCharacter = new Character();
        newCharacter.characterName = newCharacterName;
        newCharacter.className = newCharacterClass;
        newCharacter.experience = 0;
        newCharacter.level = 1;

        switch (newCharacterClass) {
            case "Tank":
                newCharacter.classType = Constants.CLASS_TANK;
                break;
            case "Gunslinger":
                newCharacter.classType = Constants.CLASS_GUNSLINGER;
                break;
        }

        //TODO - Set starting gear

        saveCharacterToDisk(newCharacter);
        allCharacters.Add(newCharacter);
    }

    public static List<Character> getCharacters() {
        return allCharacters;
    }

    public static Character getCharacter(string name) {
        foreach(Character temp in allCharacters) {
            if (temp.characterName == name) {
                return temp;
            }
        }
        return null;
    }

    public static Character getCurrentCharacter() {
        return currentCharacter;
    }

    public static void setCurrentCharacter(Character character) {
        currentCharacter = character;
    }

    public static void loadCurrentCharacterFromPrefs() {
        if (PlayerPrefs.HasKey("current_character")) {
            string characterName = PlayerPrefs.GetString("current_character");
            setCurrentCharacter(getCharacter(characterName));
        }
    }

    public static void debugPrintCharacterList() {
        foreach(Character character in allCharacters) {
            Debug.Log(character.characterName);
        }
    }
}
