using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class BaseActorTests
{
    [Test]
    public void BaseActorRequiresDamageTaker()
    {
        GameObject testObject = new GameObject("TestObject");
        testObject.AddComponent<TestGuardActor>();
        DamageTaker damageTaker = testObject.GetComponent<DamageTaker>();
        Assert.IsTrue(damageTaker != null, "Damage taker must exist for all Base Actors.");
    }

}