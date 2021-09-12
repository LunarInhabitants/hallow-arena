using Cinemachine;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class ActorController : MonoBehaviour
{
    public NetworkVariableVector3 Position = new NetworkVariableVector3(new NetworkVariableSettings
    {
        WritePermission = NetworkVariablePermission.ServerOnly,
        ReadPermission = NetworkVariablePermission.Everyone
    });

    public NetworkVariableQuaternion Rotation = new NetworkVariableQuaternion(new NetworkVariableSettings
    {
        WritePermission = NetworkVariablePermission.ServerOnly,
        ReadPermission = NetworkVariablePermission.Everyone
    });

    private float timeSinceLastPositionSync = 0.0f;

    private CinemachineVirtualCamera vCam;
    private Animator animator;
    private CharacterController characterController;

    private Vector3 targetForward = Vector3.forward;
    private float targetVelocity = 0.0f;
    private float currentVelocity = 0.0f;
    private Vector3 velocity = Vector3.zero;
    public bool IsWalking = false;

    private bool isLocalPlayer, isServer;

    public void Init(bool local, bool server)
    {
        animator = GetComponentInChildren<Animator>();
        characterController = GetComponentInChildren<CharacterController>();

        vCam = GetComponentInChildren<CinemachineVirtualCamera>();

        isLocalPlayer = local;
        isServer = server;

        if (isLocalPlayer)
        {
            vCam.LookAt = animator.GetBoneTransform(HumanBodyBones.Head);
        }
        else
        {
            Debug.Log($"Spawned player pos: {Position.Value}");
            animator.transform.position = Position.Value;
            animator.transform.rotation = Rotation.Value;
            Physics.SyncTransforms();

            vCam.enabled = false;
            GetComponent<PlayerInput>().enabled = false;
        }
        vCam.LookAt = animator.GetBoneTransform(HumanBodyBones.Head);
    }

    protected virtual void Update()
    {
        if (isLocalPlayer)
        {
            LocalUpdate();

            currentVelocity = Mathf.Lerp(currentVelocity, targetVelocity * (IsWalking ? 0.5f : 1.0f), 0.3f);
            animator.SetFloat("VelocityForward", currentVelocity);
            animator.transform.forward = Vector3.Lerp(animator.transform.forward, targetForward, 0.3f);

            SetPosition(animator.transform.position);
            SetRotation(animator.transform.rotation);
        }
        else
        {
            timeSinceLastPositionSync += Time.deltaTime;
            if (timeSinceLastPositionSync > 0.2f)
            {
                animator.transform.position = Position.Value;
                animator.transform.rotation = Rotation.Value;
                Physics.SyncTransforms();
                timeSinceLastPositionSync = 0.0f;
            }
        }
    }

    void LocalUpdate()
    {
        velocity.y = Mathf.Max(0, velocity.y - 9.81f * Time.deltaTime);
        characterController.Move(velocity * Time.deltaTime);
    }

    public void SetAnimationTrigger(string trigger)
    {
        if (isServer)
        {
            //animator.SetTrigger(trigger);
            SetAnimationTriggerClientRpc(trigger);
        }
        else
        {
            SetAnimationTriggerServerRpc(trigger);
        }
    }

    [ServerRpc]
    public void SetAnimationTriggerServerRpc(string trigger)
    {
        animator.SetTrigger(trigger);
        SetAnimationTriggerClientRpc(trigger);
    }

    [ClientRpc]
    public void SetAnimationTriggerClientRpc(string trigger)
    {
        animator.SetTrigger(trigger);
    }

    void SetPosition(Vector3 newPosition)
    {
        if (isServer)
        {
            Position.Value = newPosition;
        }
        else
        {
            _SetPositionServerRpc(newPosition);
        }
    }

    [ServerRpc]
    void _SetPositionServerRpc(Vector3 newPosition)
    {
        Position.Value = newPosition;
    }

    void SetRotation(Quaternion newRotation)
    {
        if (isServer)
        {
            Rotation.Value = newRotation;
        }
        else
        {
            _SetRotationServerRpc(newRotation);
        }
    }

    [ServerRpc]
    void _SetRotationServerRpc(Quaternion newRotation)
    {
        Rotation.Value = newRotation;
    }

    public void Jump()
    {
        if (characterController.isGrounded)
        {
            SetAnimationTrigger("Jump");
            velocity.y += 10.0f;
        }
    }
    public abstract void Attack();
    public abstract void Ability1(); //almost certainly rename
    public void Move(Vector2 movement)
    {
        float magnitude = movement.magnitude;

        if (magnitude > 0)
        {
            targetForward = new Vector3(movement.x, 0, movement.y).normalized;
        }

        targetVelocity = Mathf.Clamp01(magnitude);
    }

    public abstract void Emote1();
    public abstract void Emote2();
    public abstract void Emote3();
    public abstract void Emote4();
}
