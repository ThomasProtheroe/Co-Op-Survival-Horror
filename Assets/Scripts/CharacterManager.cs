﻿using System.Collections;
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
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(savePath + character.characterName + ".dat");
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
        allCharacters = new List<Character> ();
        foreach (string file in System.IO.Directory.GetFiles(savePath)) {
            Character temp = loadCharacterFromDisk(file);
            if (temp != null) {
                allCharacters.Add(temp);
            }
        }
    }

    public static void createNewCharacter(string newCharacterName, string newCharacterClass) {
        Character newCharacter = new Character();
        newCharacter.characterName = newCharacterName;
        newCharacter.className = newCharacterClass;
        newCharacter.experience = 0;
        newCharacter.level = 1;

        //TODO - Set starting gear

        saveCharacterToDisk(newCharacter);
        allCharacters.Add(newCharacter);
    }

    public static List<Character> getCharacters() {
        return allCharacters;
    }

    public static void setCurrentCharacter(Character character) {
        currentCharacter = character;
    }

    public static Character getCurrentCharacter() {
        return currentCharacter;
    }

    public static void debugPrintCharacterList() {
        foreach(Character character in allCharacters) {
            Debug.Log(character.characterName);
        }
    }
}
