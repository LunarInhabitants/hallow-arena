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
        float minusDamage = -1F;
        DamagePayload damagePayload = new DamagePayload(minusDamage, DamageType.Blunt);
        Assert.That(damagePayload.GetDamage, Is.EqualTo(minusDamage));
    }
}
