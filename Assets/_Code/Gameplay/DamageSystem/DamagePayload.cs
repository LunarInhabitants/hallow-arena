using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePayload
{
    private readonly int damage;
    private readonly DamageType damageType;

    public DamagePayload(int damage, DamageType damageType)
    {
        this.damage = damage;
        this.damageType = damageType;
    }

    public int GetDamage()
    {
        return damage;
    }

    public DamageType GetDamageType()
    {
        return damageType;
    }

}
