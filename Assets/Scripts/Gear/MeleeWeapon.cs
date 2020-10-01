using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : Weapon
{
    [Header("Other Attributes")]
    [SerializeField]
    private float staminaConsumption;
    [SerializeField]
    private float hitboxHeight;

    private int drawMeleeHitbox;


    void OnDrawGizmos() {
        if (drawMeleeHitbox > 0) {
            drawMeleeHitbox--;
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(new Vector3(transform.position.x + range, transform.position.y, transform.position.z), new Vector3(0.25f, hitboxHeight, 1.0f));
        }
    }

    public override void makeAttack(Attack attack) {
        Debug.Log("Making melee attack.");
        LayerMask mask = LayerMask.GetMask("Enemies");
        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(new Vector2(transform.position.x + attack.range, transform.position.y), new Vector2(0.25f, hitboxHeight), 0.0f, mask);
        int i = 0;
        while (i < hitColliders.Length)
        {
            if (hitColliders[i].gameObject.tag == "Enemy") {
                Debug.Log("Enemy hit!");
                hitColliders[i].GetComponent<EnemyController> ().takeHit(attack);
            }
            i++;
        }
    }

    public float getStaminaConsumption() {
        return staminaConsumption;
    }
}
