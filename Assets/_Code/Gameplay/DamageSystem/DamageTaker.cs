using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTaker : MonoBehaviour
{
    public List<Action<DamagePayload>> EventHandlers { get; private set; } = new List<Action<DamagePayload>>();

    [field: SerializeField] public int MaxHP { get; private set; } = 100;
    [field: SerializeField] public int CurrentHP { get; private set; } = 100;
    [field: SerializeField] public bool AllowNegativeHealth { get; private set; } = false;

    public bool IsDead => CurrentHP <= 0;


    protected void Awake()
    {
        CurrentHP = MaxHP;
    }

    /// <summary>
    /// Sets the max HP, optionally setting the current HP as well.
    /// </summary>
    /// <param name="newMax">The new maximum HP.</param>
    /// <param name="setCurrentHP">If <see langword="true"/>, also set <see cref="CurrentHP"/> to the new maximum value.</param>
    public void SetMaxHP(int newMax, bool setCurrentHP = false)
    {
        MaxHP = Mathf.Max(1, newMax);

        if(setCurrentHP)
        {
            CurrentHP = MaxHP;
        }
    }

    /// <summary>
    /// Deliver Damage Payload to entity
    /// If damage is below zero it is treated as a healing effect
    /// </summary>
    /// <param name="incomingPayload"></param>
    public void TakeDamage(DamagePayload incomingPayload)
    {
        CurrentHP = Mathf.Min(MaxHP, CurrentHP - incomingPayload.Damage);

        if(!AllowNegativeHealth && CurrentHP < 0)
        {
            CurrentHP = 0;
        }

        foreach (var callback in EventHandlers)
        {
            callback(incomingPayload);
        }
    }


    public void AddCallback(Action<DamagePayload> callback)
    {
        EventHandlers.Add(callback);
    }

}
