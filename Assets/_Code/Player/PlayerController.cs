using Cinemachine;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : NetworkBehaviour
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

    private CinemachineVirtualCamera vCam;
    private CharacterController characterController;
    private Animator animator;

    private float timeSinceLastPositionSync = 0.0f;
    private Vector3 targetForward = Vector3.forward;
    private float targetVelocity = 0.0f;
    private float currentVelocity = 0.0f;
    private bool isWalking = false;

    public override void NetworkStart()
    {
        vCam = GetComponentInChildren<CinemachineVirtualCamera>();
        characterController = GetComponentInChildren<CharacterController>();
        animator = GetComponentInChildren<Animator>();

        if (IsLocalPlayer)
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
    }

    void Update()
    {
        if (IsLocalPlayer)
        {
            LocalUpdate();

            currentVelocity = Mathf.Lerp(currentVelocity, targetVelocity * (isWalking ? 0.5f : 1.0f), 0.1f);
            animator.SetFloat("VelocityForward", currentVelocity);
            animator.transform.forward = Vector3.Lerp(animator.transform.forward, targetForward, 0.3f);

            SetPosition(animator.transform.position);
            SetRotation(animator.transform.rotation);
        }
        else
        {
            timeSinceLastPositionSync += Time.deltaTime;
            if (timeSinceLastPositionSync > 1.0f)
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

    }

    public void OnMove(InputValue inputValue)
    {
        Vector2 value = inputValue.Get<Vector2>();
        float magnitude = value.magnitude;

        if (magnitude > 0)
        {
            targetForward = new Vector3(value.x, 0, value.y).normalized;
        }

        targetVelocity = Mathf.Clamp01(magnitude);
    }

    public void OnWalk(InputValue inputValue)
    {
        isWalking = inputValue.isPressed;
    }

    public void OnEmote1(InputValue inputValue)
    {
        SetAnimationTrigger("Emote_Wave");
    }

    public void OnEmote2(InputValue inputValue)
    {
        SetAnimationTrigger("Emote_Point");
    }

    public void OnEmote3(InputValue inputValue)
    {
        SetAnimationTrigger("Emote_Insult");
    }

    public void SetAnimationTrigger(string trigger)
    {
        if (IsServer)
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
        if (IsServer)
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
        if(IsServer)
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
}
