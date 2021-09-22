using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PumpkinParasite : BaseActor
{
    [SerializeField] private Hurtbox swordHurtbox;
    [SerializeField] private PumpkinSeedProjectile pumpkinProjectile;



    public override void BeginAttack()
    {
        Animator.SetTrigger("AttackTrigger");
    }

    public override void UseAbility(int abilityIndex)
    {
        Debug.Log($"I've used ability {abilityIndex}");

        switch(abilityIndex)
        {
            case 1:
                Animator.SetTrigger("SeedVolley");
                break;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }
}
