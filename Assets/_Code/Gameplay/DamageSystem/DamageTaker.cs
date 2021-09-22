using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTaker : MonoBehaviour
{

    public List<Action<DamagePayload>> EventHandlers { get; private set; } = new List<Action<DamagePayload>>();

    [SerializeField]
    private const int startingHP = 100;
    [field: SerializeField]
    public int CurrentHP
    {
        get; private set;
    } 

    protected void Start()
    {
        CurrentHP = startingHP;
    }

    /// <summary>
    /// Deliver Damage Payload to entity
    /// If damage is below zero it is treated as a healing effect
    /// </summary>
    /// <param name="incomingPayload"></param>
    public void TakeDamage(DamagePayload incomingPayload) {
        CurrentHP -= incomingPayload.GetDamage();
        foreach (var callback in EventHandlers)
        {
            callback(incomingPayload);
        }
    }

    public bool IsDead()
    {
        return CurrentHP <= 0;
    }


    public void AddCallback(Action<DamagePayload> callback)
    {
        EventHandlers.Add(callback);
    }

}
