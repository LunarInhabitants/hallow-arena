using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePayload
{
    public int Damage { get; set; }
    public DamageType DamageType { get; set; }

    public DamagePayload() { } // Allows the use of init construction

    public DamagePayload(int damage, DamageType damageType)
    {
        this.Damage = damage;
        this.DamageType = damageType;
    }
}
