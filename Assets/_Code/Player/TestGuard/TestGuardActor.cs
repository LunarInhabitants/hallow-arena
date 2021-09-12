using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGuardActor : BaseActor
{
    public override void BeginAttack()
    {
        Debug.Log("I've swung my sword");
    }

    public override void UseAbility(int abilityIndex)
    {
        Debug.Log($"I've used ability {abilityIndex} ");
    }
}
