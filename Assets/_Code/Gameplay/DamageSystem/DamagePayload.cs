using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePayload
{
    private readonly float damage;
    private readonly DamageType damageType;

    public DamagePayload(float damage, DamageType damageType)
    {
        this.damage = damage;
        this.damageType = damageType;
    }

    public float GetDamage()
    {
        return damage;
    }

    public DamageType GetDamageType()
    {
        return damageType;
    }

}
