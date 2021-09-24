using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTaker : MonoBehaviour
{
    /// <summary>The maximum health for this entity.</summary>
    [field: SerializeField] public int MaxHP { get; private set; } = 100;

    /// <summary>The current health for this entity.</summary>
    [field: SerializeField] public int CurrentHP { get; private set; } = 100;

    /// <summary>If <see langword="true"/>, health may enter the negatives, otherwise it's clamped to 0.</summary>
    [field: SerializeField] public bool AllowNegativeHealth { get; private set; } = false;

    public bool IsDead => CurrentHP <= 0;

    private List<Action<DamagePayload>> eventHandlers = new List<Action<DamagePayload>>();


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

        foreach (var callback in eventHandlers)
        {
            callback(incomingPayload);
        }
    }

    /// <summary>
    /// Adds a callback that's invoked whenever this entity takes damage.
    /// </summary>
    /// <param name="callback">A callback to invoke when this entity takes damage.</param>
    public void AddCallback(Action<DamagePayload> callback)
    {
        eventHandlers.Add(callback);
    }

    /// <summary>
    /// Removes a previously callback that's invoked whenever this entity takes damage.
    /// </summary>
    /// <param name="callback">A callback to remove from the current event handler list.</param>
    public void RemoveCallback(Action<DamagePayload> callback)
    {
        eventHandlers.Remove(callback);
    }
}
