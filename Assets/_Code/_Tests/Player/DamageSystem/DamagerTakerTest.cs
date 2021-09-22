using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;


public class DamagerTakerTest
{
    [Test]
    public void DamageTakerTakesDamage()
    {
        GameObject testObject = new GameObject("DamageTakerGameObject");
        testObject.AddComponent<DamageTaker>();
        DamageTaker damageTaker = testObject.GetComponent<DamageTaker>();
        double initialHPBeforeDamage = damageTaker.GetCurrentDamage();
        double damage = 10D;
        DamagePayload damagePayload = new DamagePayload(damage, DamageType.Blunt);
        damageTaker.TakeDamage(damagePayload);
        Assert.That(damageTaker.GetCurrentDamage(), Is.EqualTo(initialHPBeforeDamage));
    }

}