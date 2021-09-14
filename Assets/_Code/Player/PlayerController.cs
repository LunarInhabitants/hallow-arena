using Cinemachine;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : NetworkBehaviour
{
    public static PlayerController LocalPlayerController { get; private set; }

    public BaseActor Actor { get; private set; }
    private CinemachineVirtualCamera vCam;

    // TODO: THESE ARE TEMP
    public BaseActor meleePrefab;
    public BaseActor rangedPrefab;

    public override void NetworkStart()
    {
        vCam = GetComponentInChildren<CinemachineVirtualCamera>();

        if (IsLocalPlayer)
        {
            LocalPlayerController = this;
        }
        else
        {
            vCam.enabled = false;
            GetComponent<PlayerInput>().enabled = false;
        }
    }

    void Update()
    {
        if (Actor == null)
        {
            return;
        }

        if (IsOwner)
        {
            LocalUpdate();
        }
    }

    void LocalUpdate()
    {
    }

    [Obsolete("THIS IS A DEBUG CALL")]
    public void SpawnAsMelee()
    {
        SpawnPlayerObjectServerRpc(OwnerClientId, false);
        StartCoroutine(PostCharacterSpawn());
    }

    [Obsolete("THIS IS A DEBUG CALL")]
    public void SpawnAsRanged()
    {
        SpawnPlayerObjectServerRpc(OwnerClientId, true);
        StartCoroutine(PostCharacterSpawn());
    }

    // TEST CALL
    private IEnumerator PostCharacterSpawn()
    {
        while (Actor == null)
        {
            yield return null;
            Actor = GetComponentInChildren<BaseActor>();
        }

        if (IsLocalPlayer)
        {
            vCam.Follow = Actor.transform;
            vCam.LookAt = Actor.Animator.GetBoneTransform(HumanBodyBones.Head);
        }
    }

    #region Controls Events

    public void OnMove(InputValue inputValue)
    {
        if (Actor == null)
        {
            return;
        }

        Vector2 value = inputValue.Get<Vector2>();
        float magnitude = value.magnitude;

        Vector3 moveVec = Vector3.zero;
        if (magnitude > 0)
        {
            moveVec = new Vector3(value.x, 0, value.y).normalized;
            Actor.SetForward(moveVec); // TEMP UNTIL 3RD PERSON CAMERA
        }

        //Actor.SetOrientatedMovementVector(moveVec);
        Actor.SetOrientatedMovementVector(new Vector3(0, 0, magnitude)); // TEMP UNTIL 3RD PERSON CAMERA
    }

    public void OnJump(InputValue inputValue)
    {
        if (Actor != null)
        {
            Actor.Jump();
        }
    }

    public void OnWalk(InputValue inputValue)
    {
        if (Actor != null)
        {
            Actor.SetMoveSpeedMultiplier(inputValue.isPressed ? 0.5f : 1.0f);
        }
    }

    public void OnAttack(InputValue inputValue)
    {
        if (Actor != null)
        {
            if (inputValue.isPressed)
            {
                Actor.BeginAttack();
            }
            else
            {
                Actor.EndAttack();
            }
        }
    }

    public void OnAbility1(InputValue inputValue)
    {
        if (Actor != null)
        {
            Actor.UseAbility(1);
        }
    }

    public void OnAbility2(InputValue inputValue)
    {
        if (Actor != null)
        {
            Actor.UseAbility(2);
        }
    }

    public void OnAbility3(InputValue inputValue)
    {
        if (Actor != null)
        {
            Actor.UseAbility(3);
        }
    }

    public void OnAbility4(InputValue inputValue)
    {
        if (Actor != null)
        {
            Actor.UseAbility(4);
        }
    }

    public void OnEmote1(InputValue inputValue)
    {
        if (Actor != null)
        {
            Actor.PlayEmote(1);
        }
    }

    public void OnEmote2(InputValue inputValue)
    {
        if (Actor != null)
        {
            Actor.PlayEmote(2);
        }
    }

    public void OnEmote3(InputValue inputValue)
    {
        if (Actor != null)
        {
            Actor.PlayEmote(3);
        }
    }

    public void OnEmote4(InputValue inputValue)
    {
        if (Actor != null)
        {
            Actor.PlayEmote(4);
        }
    }

    #endregion Controls Events

    [ServerRpc]
    public void SpawnPlayerObjectServerRpc(ulong clientID, bool isRanged)
    {
        BaseActor ba = Instantiate(isRanged ? rangedPrefab : meleePrefab, transform);
        ba.GetComponent<NetworkObject>().SpawnWithOwnership(clientID);
        //ba.GetComponent<NetworkObject>().Spawn();
    }
}
