using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;


public class DamagePayloadTest
{
    [Test]
    public void DamagePayloadCanBeSetToMinusToAllowHealing()
    {
        int minusDamage = -1;
        DamagePayload damagePayload = new DamagePayload(minusDamage, DamageType.Blunt);
        Assert.That(damagePayload.Damage, Is.EqualTo(minusDamage));
    }
}
