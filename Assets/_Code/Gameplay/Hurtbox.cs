using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void OnHurtboxHit(Collider other, int damage, int damageType);

[RequireComponent(typeof(Collider))]
public class Hurtbox : MonoBehaviour
{
    [SerializeField] private Collider hurtCollider;
    [SerializeField] private bool activeOnSpawn = false;
    [SerializeField] private int damageOnHurt = 20;
    // TODO: Convert to a damage type enum
    [SerializeField] private int damageType = 0;
    [SerializeField] private bool canHurtOwner = false;

    public GameObject Owner { get; private set; }
    private List<OnHurtboxHit> onHitEvents = new List<OnHurtboxHit>();

    protected void Awake()
    {
        if (hurtCollider == null)
        {
            hurtCollider = GetComponent<Collider>();
        }

        hurtCollider.isTrigger = true;

        if(activeOnSpawn)
        {
            Activate();
        }
        else
        {
            Deactivate();
        }
    }

    public void SetOwner(GameObject newOwner)
    {
        Owner = newOwner;
    }

    /// <summary>
    /// If set, callback is triggered when the hurtbox hits a character.
    /// </summary>
    /// <param name="callback">The callback to call on hit.</param>
    public void AddOnHitCallback(OnHurtboxHit callback)
    {
        onHitEvents.Add(callback);
    }

    public void Activate()
    {
        hurtCollider.enabled = true;
    }

    public void Deactivate()
    {
        hurtCollider.enabled = false;
    }

    protected void OnTriggerEnter(Collider other)
    {
        if(!canHurtOwner && other.gameObject == Owner)
        {
            return;
        }

        // TODO: Use DamageHandler
        // var dmgHandler = other.gameObject.GetComponent<DamageHandler>();
        // dmgHandler.ApplyDamage(damageOnHurt, damageType, Owner);

        Debug.Log($"{Owner} hurt {other.gameObject} for {damageOnHurt} damage of type {damageType}");

        foreach(var onHit in onHitEvents)
        {
            onHit?.Invoke(other, damageOnHurt, damageType);
        }
    }

#if UNITY_EDITOR
    protected void OnDrawGizmos()
    {
        if(hurtCollider == null)
        {
            return;
        }

        Gizmos.matrix = transform.localToWorldMatrix;

        if (hurtCollider is BoxCollider bc)
        {
            Gizmos.color = Color.red * 0.25f;
            Gizmos.DrawCube(Vector3.zero, bc.size);
            Gizmos.color = Color.red * 0.5f;
            Gizmos.DrawWireCube(Vector3.zero, bc.size);
        }
        else if (hurtCollider is SphereCollider sc)
        {
            Gizmos.color = Color.red * 0.25f;
            Gizmos.DrawSphere(Vector3.zero, sc.radius);
            Gizmos.color = Color.red * 0.5f;
            Gizmos.DrawWireSphere(Vector3.zero, sc.radius);
        }
        else if (hurtCollider is MeshCollider mc)
        {
            Gizmos.color = Color.red * 0.25f;
            Gizmos.DrawMesh(mc.sharedMesh);
            Gizmos.color = Color.red * 0.5f;
            Gizmos.DrawMesh(mc.sharedMesh);
        }
    }
#endif
}
