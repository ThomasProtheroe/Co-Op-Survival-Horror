using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedWeapon : Weapon
{

    [Header("Other Attributes")]
    [SerializeField]
    private int ammoCapacity;
    [SerializeField]
    private int reserveAmmoCapacity;


    #region private fields

    private int currentAmmo;
    private int currentReserveAmmo;

    #endregion


    // Start is called before the first frame update
    void Start()
    {
        weaponType = 1;
        currentAmmo = ammoCapacity;
        currentReserveAmmo = reserveAmmoCapacity;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Attack fireWeapon() {
        if (currentAmmo <= 0) {
            return null;
        }

        currentAmmo -= 1;

        Attack attack = new Attack();
        attack.damage = damage;
        attack.range = range;
        attack.armorPiercing = armorPiercing;
        attack.punchThrough = punchThrough;
        attack.knockBack = knockback;

        return attack;
    }

    public void reloadWeapon() {
        if (currentReserveAmmo == 0 || currentAmmo == ammoCapacity) {
            return;
        }

        int ammoNeeded = ammoCapacity - currentAmmo;
        if (currentReserveAmmo >= ammoNeeded) {
            currentAmmo = ammoCapacity;
            currentReserveAmmo -= ammoNeeded;
        } else {
            currentAmmo += currentReserveAmmo;
            currentReserveAmmo = 0;
        }
    }

    public int getCurrentAmmo() {
        return currentAmmo;
    }

    public int getAmmoCapacity() {
        return ammoCapacity;
    }

    public int getcurrentReserveAmmo() {
        return currentReserveAmmo;
    }
}
