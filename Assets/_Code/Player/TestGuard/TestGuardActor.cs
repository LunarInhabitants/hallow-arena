using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGuardActor : BaseActor
{
    public override void BeginAttack()
    {
        Debug.Log("I've swung my sword");
        Animator.SetTrigger("AttackTrigger");
    }

    /// <summary>
    /// Note: Triggered from animation.
    /// </summary>
    public void DoCastAttack()
    {
        // Perform a hit raycast or turn on a hurtbox.
        Debug.Log("I've triggered a hit!");
    }

    public override void UseAbility(int abilityIndex)
    {
        Debug.Log($"I've used ability {abilityIndex} ");
    }
}
