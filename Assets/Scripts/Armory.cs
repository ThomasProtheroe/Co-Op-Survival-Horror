using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

//using BayatGames.Serialization.Formatters.Json;

public static class Armory
{
    private static Dictionary<string, Dictionary<string, float>> rangedWeapons;
    private static Dictionary<string, Dictionary<string, float>> meleeWeapons;

    public static void loadGear() {
        loadWeapons();
    }

    private static void loadWeapons() {
        rangedWeapons = new Dictionary<string, Dictionary<string, float>> ();
        string path = "Assets/Content/Gear/rangedweapons.json";
        var jsonText = System.IO.File.ReadAllText(@path);
        
        JArray data = (JArray)JsonConvert.DeserializeObject(jsonText);
        Dictionary<string, float> weaponDict;

        foreach(var jsonWeapon in data.Children()) {
            weaponDict = new Dictionary<string, float> ();
            if (jsonWeapon.SelectToken("attributes") != null) {
                foreach(JProperty property in jsonWeapon.SelectToken("attributes").Children<JProperty>()) {
                    weaponDict.Add(property.Name, (float)property.Value);
                }
            }
            weaponDict.Add("weaponType", 1.0f);
            rangedWeapons.Add((string)jsonWeapon.SelectToken("weaponName"), weaponDict);
        }

        meleeWeapons = new Dictionary<string, Dictionary<string, float>> ();
        path = "Assets/Content/Gear/meleeWeapons.json";
        jsonText = System.IO.File.ReadAllText(@path);
        
        data = (JArray)JsonConvert.DeserializeObject(jsonText);

        foreach(var jsonWeapon in data.Children()) {
            weaponDict = new Dictionary<string, float> ();
            if (jsonWeapon.SelectToken("attributes") != null) {
                foreach(JProperty property in jsonWeapon.SelectToken("attributes").Children<JProperty>()) {
                    weaponDict.Add(property.Name, (float)property.Value);
                }
            }
            weaponDict.Add("weaponType", 0f);
            meleeWeapons.Add((string)jsonWeapon.SelectToken("weaponName"), weaponDict);
        }
    }

    public static Dictionary<string, float> getWeapon(string weaponName) {
        if (rangedWeapons.ContainsKey(weaponName)) {
            return rangedWeapons[weaponName];
        } else if (meleeWeapons.ContainsKey(weaponName)) {
            return meleeWeapons[weaponName];
        }

        return null;
    }
}
