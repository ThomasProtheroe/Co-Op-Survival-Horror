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
    [SerializeField]
    protected float attackDelay;
    protected int weaponType;
    protected float attackCooldown;
    protected bool ready;

    // Update is called once per frame
    void Update()
    {
        if (attackCooldown > 0.0f) {
            attackCooldown -= Time.deltaTime;
            if (attackCooldown <= 0.0f) {
                attackCooldown = 0.0f;
                ready = true;
            }
        }
    }

    public virtual Attack prepAttack() {
        Attack attack = new Attack();
        attack.damage = damage;
        attack.range = range;
        attack.armorPiercing = armorPiercing;
        attack.punchThrough = punchThrough;
        attack.knockBack = knockback;

        return attack;
    }

    public virtual void makeAttack(Attack attack) {
        return;
    }

    public int getWeaponType() {
        return weaponType;
    }

    public string getWeaponName() {
        return weaponName;
    }

    public bool isReady() {
        return ready;
    }

}
