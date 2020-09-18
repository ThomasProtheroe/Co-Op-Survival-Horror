using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    [Header("Basic Attributes")]
    [SerializeField]
    private int maxHp;
    [SerializeField]
    private float baseArmor;
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float awareness;

    [Header("Attack Attributes")]
    [SerializeField]
    private int attackDamage;

    #region Private fields

    //Enemy attributes
    private int currentHp;
    private float currentMoveSpeed;
    private float alertness;


    #endregion

    // Start is called before the first frame update
    void Start()
    {
        currentHp = maxHp;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void takeHit(Attack attack) {
        takeDamage(attack);
    }

    private void takeDamage(Attack attack) {
        int modifiedDamage = (int)((float)attack.damage * (1.0f - (baseArmor / 100.0f)));
        currentHp -= modifiedDamage;

        if (currentHp <= 0) {
            killEnemy();
        }
    }

    private void killEnemy() {
        Destroy(gameObject);
    }
}
