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
    [SerializeField]
    private float reloadTime;


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

    public override Attack prepAttack() {
        if (currentAmmo <= 0) {
            return null;
        }

        currentAmmo -= 1;

        return base.prepAttack();
    }

    public override void makeAttack(Attack attack) {
        LayerMask mask = LayerMask.GetMask("Enemies", "Terrain");
        RaycastHit2D hit = Physics2D.Raycast(transform.position, attack.direction, attack.range, mask);
        if (hit.collider != null) {
            GameObject targetHit = hit.collider.gameObject;
            if (targetHit.tag == "Enemy") {
                targetHit.GetComponent<EnemyController> ().takeHit(attack);
            }
        }
        Debug.Log("Shot fired - " + getCurrentAmmo() + " rounds left in the magazine.");
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
