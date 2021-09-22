using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTaker : MonoBehaviour
{
    [SerializeField]
    private const float startingHP = 100F;

    [field: SerializeField]
    public float CurrentHP
    {
        get; private set;
    } 

    private void Start()
    {
        CurrentHP = startingHP;
    }

    public void TakeDamage(DamagePayload incomingDamage) {
        CurrentHP -= incomingDamage.GetDamage();
    }

}
