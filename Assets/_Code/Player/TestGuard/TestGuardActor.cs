using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGuardActor : BaseActor
{
    [SerializeField] private Hurtbox swordHurtbox;

    protected override void Awake()
    {
        base.Awake();
        swordHurtbox.SetOwner(gameObject);
    }

    public override void BeginAttack()
    {
        Debug.Log("I've swung my sword");
        Animator.SetTrigger("AttackTrigger");
    }

    /// <summary>
    /// Note: Triggered from animation.
    /// </summary>
    public void DoSwordAttack()
    {
        StartCoroutine(HandleSwordAttack());
    }

    private IEnumerator HandleSwordAttack()
    {
        swordHurtbox.Activate();
        yield return new WaitForSeconds(0.1f);
        swordHurtbox.Deactivate();
    }

    public override void UseAbility(int abilityIndex)
    {
        Debug.Log($"I've used ability {abilityIndex} ");
    }
}
