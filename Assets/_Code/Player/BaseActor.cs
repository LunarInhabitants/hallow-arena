using MLAPI;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;
using MLAPI.Prototyping;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The basic implementation of a character 'actor'.<para/>
/// This essentially acts like a puppet to be controlled by a parent controller, either a player or an AI.
/// </summary>
[RequireComponent(typeof(Animator), typeof(CharacterController), typeof(DamageTaker))]
[RequireComponent(typeof(NetworkObject), typeof(NetworkTransform), typeof(NetworkAnimator))]
public abstract partial class BaseActor : NetworkBehaviour
{
    const float PLAYER_MOVEMENT_ACCEL = 8.0f;
    const float IMPULSE_DECAY_MULT = 5.0f;
    /// <summary>This is what speed the x and z impulese velocities decay to when in the air, rather than 0.</summary>
    const float MAX_AIR_IMPULSE_SPEED = 2.0f;

    private static bool hasDoneStaticInit = false;
    public static LayerMask LayerMask_PlayerCharacter { get; private set; }
    public static LayerMask LayerMask_OtherCharacter { get; private set; }
    public static LayerMask LayerMask_Projectile { get; private set; }

    #region Variables

    [SerializeField] private ActorUI actorUIPrefab;
    public ActorUI ActorUI { get; private set; }

    [SerializeField] private PlayerNameplate nameplatePrefab;
    public PlayerNameplate Nameplate { get; private set; }


    [SerializeField] private int initialHealth;
    [field: SerializeField] public ETeam Team { get; private set; } = ETeam.NoTeam;

    #endregion Variables

    #region Components

    /// <summary>
    /// The direct parent controller for this actor.
    /// </summary>
    public PlayerController ParentController { get; private set; }

    /// <summary>
    /// This character's camera rig. Only valid on a local player.
    /// </summary>
    public PlayerCameraRig CameraRig { get; private set; }

    /// <summary>
    /// The Unity <see cref="Animator"/> attached to this Actor.
    /// </summary>
    public Animator Animator { get; private set; }

    /// <summary>
    /// The Unity <see cref="CharacterController"/> attached to this Actor.
    /// </summary>
    public CharacterController CharacterController { get; private set; }

    /// <summary>
    /// Handles the heatlh and damage of this actor.
    /// </summary>
    public DamageTaker DamageTaker { get; private set; }

    #endregion Components

    /// <summary>
    /// A vector describing 3D motion of the character, with +Z being forwards from where I'm looking and +X being right.
    /// </summary>
    public Vector3 OrientatedMovementVector { get; private set; }
    private Vector2 cameraLookAngles = new Vector2();
    public Vector3 LookDirection { get; private set; } = Vector3.forward;
    private Vector3 lastOrientatedVelocity = Vector3.zero;

    /// <summary>
    /// A multiplier for the movement speed. Adjust if walking, there's a powerup etc.
    /// </summary>
    public float MoveSpeedMultiplier { get; private set; } = 1.0f;
    private float lastMoveSpeedMultiplier = 1.0f;

    /// <summary>
    /// Current force applied to the character.
    /// </summary>
    public Vector3 Impulse { get; private set; }

    public bool IsDead => DamageTaker.CurrentHP <= 0;
    public string PlayerName => ParentController?.PlayerName ?? "Player";

    /// <summary>
    /// The list of emotes. Emote 1 = Index 0.<para/>
    /// TODO: Make this player configurable.
    /// </summary>
    [field: SerializeField] public string[] Emotes { get; private set; } = new[] { "Emote_Wave", "Emote_Point", "Emote_Insult" };

    protected virtual void Awake()
    {
        // Needed here as Unity doesn't like calling some functions outside of Awake.
        if (!hasDoneStaticInit)
        {
            hasDoneStaticInit = true;

            LayerMask_PlayerCharacter = LayerMask.GetMask("PlayerCharacter");
            LayerMask_OtherCharacter = LayerMask.GetMask("OtherCharacter");
            LayerMask_Projectile = LayerMask.GetMask("Projectile");
        }

        CharacterController = GetComponent<CharacterController>();
        Animator = GetComponent<Animator>();
        DamageTaker = GetComponent<DamageTaker>();
        DamageTaker.SetMaxHP(initialHealth, true);
        DamageTaker.AddCallback(OnHurt);
    }

    public override void NetworkStart()
    {
        base.NetworkStart();

        if(IsOwner)
        {
            transform.SetLayerRecursively(LayerMask_PlayerCharacter);

            ActorUI = Instantiate(actorUIPrefab, transform);
            // TODO: Init the health value from the DamageHandler instead.
            ActorUI.Init(initialHealth);
        }
        else
        {
            transform.SetLayerRecursively(LayerMask_OtherCharacter);

            // Only other players get nameplates
            Nameplate = Instantiate(nameplatePrefab, transform);
            Transform headBone = Animator.GetBoneTransform(HumanBodyBones.Head);
            Nameplate.transform.localPosition = new Vector3(0.0f, headBone.position.y - transform.position.y + 0.5f, 0.0f);
        }


        Teleport(_Net_Position.Value, _Net_LookDirection.Value);
    }

    public void OnCreatedByPlayerController(PlayerController creator)
    {
        ParentController = creator;

        if (Nameplate != null)
        {
            Nameplate.SetName(PlayerName);
        }
    }

    public void OnCameraRigAttached(PlayerCameraRig rig)
    {
        CameraRig = rig;
    }

    protected void Update()
    {
        PreNetworkUpdate();

        if (!IsDead)
        {
            if (IsOwner)
            {
                LocalUpdate();
            }

            if (Impulse.sqrMagnitude > 0.01f)
            {
                CharacterController.Move(Impulse * Time.deltaTime);

                // Decay the impulse force
                Vector3 newImpulse = Impulse;

                // Decay by gravity
                newImpulse.y = Mathf.Max(0.0f, newImpulse.y - 9.81f * Time.deltaTime);

                // Decay by group friction if grounded, or air friction if airborne.
                float delta = Time.deltaTime * IMPULSE_DECAY_MULT;
                bool isGrounded = CharacterController.isGrounded; // Cache the value - isGrounded might raycast each time.
                                                                  //float targetVelX = isGrounded ? 0.0f : Mathf.Max(Mathf.Abs(newImpulse.x), MAX_AIR_IMPULSE_SPEED) * Mathf.Sign(newImpulse.x);
                                                                  //float targetVelZ = isGrounded ? 0.0f : Mathf.Max(Mathf.Abs(newImpulse.z), MAX_AIR_IMPULSE_SPEED) * Mathf.Sign(newImpulse.z);
                float targetVelX = 0.0f;
                float targetVelZ = 0.0f;
                newImpulse.x = Mathf.Lerp(newImpulse.x, targetVelX, delta);
                newImpulse.z = Mathf.Lerp(newImpulse.z, targetVelZ, delta);

                Impulse = newImpulse;
            }
        }

        PostNetworkUpdate();
    }

    /// <summary>
    /// Code that should only be ran by the local player.
    /// </summary>
    protected virtual void LocalUpdate()
    {
        Vector3 targetForward = LookDirection;
        targetForward.y = 0.0f;
        transform.forward = targetForward.normalized;

        if (CameraRig != null && CameraRig.CameraMode != CameraMode.IsometricCamera && !ParentController.IsUIOpen) // TODO: Dev check. Remove this block if isometric camera is removed
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        // Orientates the animations based on true velocity - Means things like capes and such can be affected by knockback.
        Vector3 currentOrientatedVelocity = OrientatedMovementVector + transform.rotation * Impulse;
        lastOrientatedVelocity = Vector3.MoveTowards(lastOrientatedVelocity, currentOrientatedVelocity, Time.deltaTime * PLAYER_MOVEMENT_ACCEL);

        lastMoveSpeedMultiplier = Mathf.Lerp(lastMoveSpeedMultiplier, MoveSpeedMultiplier, 0.2f);

        Animator.SetFloat("VelocityForward", lastOrientatedVelocity.z * lastMoveSpeedMultiplier);
        Animator.SetFloat("VelocityRight", lastOrientatedVelocity.x * lastMoveSpeedMultiplier);
    }

    protected void OnAnimatorIK(int layerIndex)
    {
        Transform head = Animator.GetBoneTransform(HumanBodyBones.Head);
        if (head != null)
        {
            Animator.SetLookAtPosition(head.position + LookDirection);
            Animator.SetLookAtWeight(1.0f, 0.5f, 1.0f);
        }
    }

    public override string ToString()
    {
        return $"{PlayerName} - {base.ToString()}";
    }

    protected virtual void OnHurt(DamagePayload payload)
    {
        Debug.Log($"{this} took {payload.Damage} {payload.DamageType} damage");
        ActorUI.SetCurrentHealth(Mathf.Max(0, DamageTaker.CurrentHP));

        if(DamageTaker.CurrentHP <= 0)
        {
            OnDied();
        }
    } 

    protected virtual void OnDied()
    {
        Debug.Log($"{this} died!");

        lastOrientatedVelocity = Vector3.zero;
        // TODO: Handle dying
    }

    protected virtual void Respawn()
    {
        SpawnPoint sp = BaseGameMode.Instance.GetBestSpawnPointForRespawn(Team);
        DamageTaker.TakeDamage(new DamagePayload
        {
            Damage = -9999,
            DamageType = DamageType.Generic,
        });
        sp.Spawn(this);
    }

    #region General Control

    public void SetLookVector(Vector3 newLookForward)
    {
        LookDirection = newLookForward;
    }

    public void AddLookVector(Vector3 newLookForward)
    {
        LookDirection += newLookForward;
    }

    public void AddLookVectorFromInput(Vector2 lookInput)
    {
        // TODO: Handle look sensitivity
        cameraLookAngles += lookInput * 0.05f;
        cameraLookAngles.y = Mathf.Clamp(cameraLookAngles.y, -75.0f, 75.0f);

        LookDirection = Quaternion.Euler(-cameraLookAngles.y, cameraLookAngles.x, 0.0f) * Vector3.forward;
    }

    /// <summary>
    /// Makes the character jump.
    /// </summary>
    public void Jump()
    {
        if(CharacterController.isGrounded)
        {
            ApplyImpulse(CharacterController.velocity + new Vector3(0.0f, 10.0f, 0.0f));
            SetAnimationTrigger("Jump");
        }
    }

    /// <summary>
    /// Called by <see cref="PlayerController"/> and handles respawning, as well as calling the actual attack function <see cref="BeginAttack"/>.
    /// </summary>
    public void DoBeginAttack()
    {
        if (IsDead)
        {
            Respawn();
        }
        else
        {
            BeginAttack();
        }
    }

    /// <summary>
    /// Triggers the basic player attack.<para/>
    /// For characters that repeatedly attack when held, this starts the attack sequence.
    /// </summary>
    public abstract void BeginAttack();

    /// <summary>
    /// Stops the attack if the a character performs a continuous attack.
    /// </summary>
    public virtual void EndAttack()
    {
        
    }

    /// <summary>
    /// Uses a specified ability, with the index provided from player input.<para/>
    /// Note: The index system is to allow characters with 2, 3, 4 etc abilities.<para/>
    /// Note: We should not down which key should map to which index.
    /// </summary>
    /// <param name="abilityIndex">The index of the ability to run.</param>
    public abstract void UseAbility(int abilityIndex);

    /// <summary>
    /// Plays an emote based on emote index.<para/>
    /// Note: Emotes should ideally be reorderable by the player, hence the index system.
    /// </summary>
    /// <param name="emoteIndex"></param>
    public void PlayEmote(int emoteIndex)
    {
        if(emoteIndex > 0 && emoteIndex <= Emotes.Length)
        {
            SetAnimationTrigger(Emotes[emoteIndex - 1]);
        }

        // TESTING
        if(ActorUI != null)
        {
            if (emoteIndex == 1)
            {
                // TEST HEAL
                DamagePayload payload = new DamagePayload(-10, DamageType.Generic);
                DamageTaker.TakeDamage(payload);
            }
            else if(emoteIndex == 2)
            {
                // TEST HURT
                DamagePayload payload = new DamagePayload(10, DamageType.Blunt);
                DamageTaker.TakeDamage(payload);
            }
        }
    }


    #endregion

    #region Physics

    public void Teleport(Vector3 position, Vector3 forward)
    {
        SetPosition(position);
        SetLookDirection(forward);
        Physics.SyncTransforms(); // Needed as CharacterController otherwise won't update it's internal state.
    }

    public void ApplyImpulse(Vector3 impulse)
    {
        Impulse += impulse;
    }

    public void SetOrientatedMovementVector(Vector3 movementVector)
    {
        OrientatedMovementVector = movementVector;
    }

    public void AddOrientatedMovementVector(Vector3 movementVector)
    {
        OrientatedMovementVector += movementVector;
    }

    public void SetForward(Vector3 forward)
    {
        LookDirection = forward;
    }

    public void SetMoveSpeedMultiplier(float newMult)
    {
        MoveSpeedMultiplier = Mathf.Max(0.0f, newMult);
    }

    #endregion Physics

    #region Networked Positon/Rotation
    void SetPosition(Vector3 newPosition)
    {
        if(IsOwner)
        {
            transform.position = newPosition;

            if (IsServer)
            {
                _Net_Position.Value = newPosition;
            }
            else
            {
                _SetPositionServerRpc(newPosition);
            }
        }
    }

    [ServerRpc]
    void _SetPositionServerRpc(Vector3 newPosition)
    {
        _Net_Position.Value = newPosition;
    }

    void SetLookDirection(Vector3 newLook)
    {
        if (IsOwner)
        {
            LookDirection = newLook.normalized;
            Vector3 lookDirHoriz = LookDirection;
            lookDirHoriz.y = 0;
            cameraLookAngles.x = Vector3.SignedAngle(Vector3.forward, lookDirHoriz.normalized, Vector3.up);

            if (IsServer)
            {
                _Net_LookDirection.Value = newLook;
            }
            else
            {
                _SetLookDirectionServerRpc(newLook);
            }
        }
    }

    [ServerRpc]
    void _SetLookDirectionServerRpc(Vector3 newLook)
    {
        _Net_LookDirection.Value = newLook;
    }

    #endregion Networked Position/Rotation

    #region Animation

    public void SetAnimationTrigger(string trigger)
    {
        if (IsServer)
        {
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
        Animator.SetTrigger(trigger);
        SetAnimationTriggerClientRpc(trigger);
    }

    [ClientRpc]
    public void SetAnimationTriggerClientRpc(string trigger)
    {
        Animator.SetTrigger(trigger);
    }

    #endregion
}
