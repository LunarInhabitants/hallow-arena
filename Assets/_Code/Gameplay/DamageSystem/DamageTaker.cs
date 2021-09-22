using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTaker : MonoBehaviour
{
    [SerializeField]
    private const double startingHP = 100D;

    private double currentHP;

    private void Start()
    {
        currentHP = startingHP;
    }

    public void TakeDamage(DamagePayload incomingDamage) { 
        
    }

    public double GetCurrentDamage()
    {
        return currentHP;
    }
}
