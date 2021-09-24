using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRangedActor : BaseActor
{
    [SerializeField] private TestRangedProjectile castProjectilePrefab;
    [SerializeField] private Transform castPoint;

    public override void BeginAttack()
    {
        Animator.SetBool("AttackHeld", true);
    }

    public override void EndAttack()
    {
        Animator.SetBool("AttackHeld", false);
    }

    /// <summary>
    /// Note: Triggered from animation.
    /// </summary>
    public void DoCastAttack()
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
        TestRangedProjectile proj = Instantiate(castProjectilePrefab, castPoint.position, Quaternion.identity);
        proj.Init(gameObject, direction);
    }

    public override void UseAbility(int abilityIndex)
    {
        Debug.Log($"I've used ability {abilityIndex}");
    }
}
