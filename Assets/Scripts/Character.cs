using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Character
{
    public string characterName;
    public int classType;
    public string className;
    public int level;
    public int experience;
    public List<string> weapons;
    public string armor;

    public void addWeapon(string inWeapon) {
        if (weapons == null) {
            weapons = new List<string> ();
        }
        weapons.Add(inWeapon);
    }

    public void addArmor(string inArmor) {
        armor = inArmor;
    }
}
