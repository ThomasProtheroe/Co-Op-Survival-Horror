using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armor : MonoBehaviour
{
    [Header("Basic Attributes")]
    [SerializeField]
    private string armorName;
    [SerializeField]
    private float armorValue;
    [SerializeField]
    private float moveSpeedModifier;
    [SerializeField]
    private float staminaCostModifier;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public float getArmorValue() {
        return armorValue;
    }

    public float getMoveSpeedModifier() {
        return moveSpeedModifier;
    }

    public float getStaminaCostModifier() {
        return staminaCostModifier;
    }

}
