using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRangedActor : BaseActor
{
    public override void BeginAttack()
    {
        Debug.Log("I've started shooting");
    }

    public override void EndAttack()
    {
        Debug.Log("I've stopped shooting");
    }

    public override void UseAbility(int abilityIndex)
    {
        Debug.Log($"I've used ability {abilityIndex}");
    }
}
