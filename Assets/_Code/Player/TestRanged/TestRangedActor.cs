using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRangedActor : BaseActor
{
    public override void BeginAttack()
    {
        Debug.Log("I've started shooting");
        Animator.SetBool("AttackHeld", true);
    }

    public override void EndAttack()
    {
        Debug.Log("I've stopped shooting");
        Animator.SetBool("AttackHeld", false);
    }

    /// <summary>
    /// Note: Triggered from animation.
    /// </summary>
    public void DoCastAttack()
    {
        // Spawn a cast attack
        Debug.Log("I've cast!");
    }

    public override void UseAbility(int abilityIndex)
    {
        Debug.Log($"I've used ability {abilityIndex}");
    }
}
