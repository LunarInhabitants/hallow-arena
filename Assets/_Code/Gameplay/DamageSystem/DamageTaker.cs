using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTaker : MonoBehaviour
{
    [SerializeField]
    private const float startingHP = 100F;

    private float currentHP;

    private void Start()
    {
        currentHP = startingHP;
    }

    public void TakeDamage(DamagePayload incomingDamage) { 
        
    }

    public float GetCurrentDamage()
    {
        return currentHP;
    }
}
