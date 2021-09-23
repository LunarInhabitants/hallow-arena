using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PumpkinParasite : BaseActor
{
    [SerializeField] private Hurtbox slashHurtbox;
    [SerializeField] private PumpkinSeedProjectile pumpkinProjectilePrefab;
    [SerializeField] private Transform castPoint;

    public void Start()
    {
        slashHurtbox.SetOwner(gameObject);
    }

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
                Animator.SetTrigger("VolleyTrigger");
                break;
        }
    }

    public void StartSlashAttack()
    {
        slashHurtbox.Activate();
    }

    public void EndSlashAttack()
    {
        slashHurtbox.Deactivate();
    }

    public void DoSeedVolley()
    {
        Vector3 direction;
        if (Physics.Raycast(CameraRig.VirtualCamera.transform.position, CameraRig.VirtualCamera.transform.forward, out RaycastHit hit, 1024.0f, ~(LayerMask_PlayerCharacter | LayerMask_Projectile)))
        {
            Debug.Log(hit.collider);
            Debug.Log(hit.collider.gameObject);
            Vector3 target = hit.point;
            direction = (target - castPoint.position).normalized;
        }
        else
        {
            direction = CameraRig.VirtualCamera.transform.forward.normalized;
        }

        // Projectile pooling?
        PumpkinSeedProjectile proj = Instantiate(pumpkinProjectilePrefab, castPoint.position, Quaternion.identity);
        proj.Init(gameObject, direction);
    }
}
