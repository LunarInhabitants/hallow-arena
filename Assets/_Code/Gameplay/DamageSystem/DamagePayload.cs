using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePayload
{
    private double damage;
    private DamageType damageType;

    public DamagePayload(double damage, DamageType damageType)
    {
        this.damage = damage;
        this.damageType = damageType;
    }

    public double GetDamage()
    {
        return damage;
    }

    public DamageType GetDamageType()
    {
        return damageType;
    }

}
