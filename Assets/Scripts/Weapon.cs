using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Basic Attributes")]
    [SerializeField]
    protected string weaponName;

    [Header("Attack Attributes")]
    [SerializeField]
    protected int damage;
    [SerializeField]
    protected int armorPiercing;
    [SerializeField]
    protected int punchThrough;
    [SerializeField]
    protected int knockback;
    [SerializeField]
    protected float range;
    protected int weaponType;

    public int getWeaponType() {
        return weaponType;
    }

    public string getWeaponName() {
        return weaponName;
    }

}
