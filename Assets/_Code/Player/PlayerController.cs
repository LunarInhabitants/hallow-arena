using Cinemachine;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : NetworkBehaviour
{
    public static PlayerController LocalPlayerController { get; private set; }

    [SerializeField] private GameDatabase gameDatabase;
    [SerializeField] private PlayerCameraRig playerCameraRigPrefab;
    [SerializeField] private CharacterSelectUI characterSelectUIPrefab;

    private PlayerSettings playerSettings;
    private NetworkVariableString desiredName = new NetworkVariableString(new NetworkVariableSettings { WritePermission = NetworkVariablePermission.OwnerOnly });
    private NetworkVariableString uniqueName = new NetworkVariableString(new NetworkVariableSettings { WritePermission = NetworkVariablePermission.ServerOnly });

    public bool IsUIOpen => characterSelectUI.gameObject.activeInHierarchy;
    public BaseActor Actor { get; private set; }
    public PlayerCameraRig CameraRig { get; private set; }
    private CharacterSelectUI characterSelectUI;

    public override void NetworkStart()
    {
        if (IsServer)
        {
            desiredName.OnValueChanged += desiredNameChanged;
        }
        uniqueName.OnValueChanged += uniqueNameChanged;

        if (IsLocalPlayer)
        {
            playerSettings = FindObjectOfType<PlayerSettings>();

            desiredName.Value = playerSettings ? playerSettings.GetName() : "Player";

            LocalPlayerController = this;
            CameraRig = Instantiate(playerCameraRigPrefab, transform);
            CameraRig.SetMapVantageCamera(null); // Sets to the first vantage camera, if available.
            characterSelectUI = Instantiate(characterSelectUIPrefab, transform);
        }
        else
        {   
            GetComponent<PlayerInput>().enabled = false;
        }
    }
    private void uniqueNameChanged(string previousValue, string newValue)
    {
        Debug.Log($"OwnerClient {OwnerClientId} name changed from \"{previousValue}\" to \"{uniqueName.Value}\"");
    }

    private void desiredNameChanged(string previousValue, string newValue)
    {
        
        Debug.Log($"OwnerClient {OwnerClientId} requested name change from \"{previousValue}\" to \"{newValue}\"");
        uniqueName.Value = validateName(newValue);
    }

    private string validateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) { return "Empty"; };
        return name;
    }

    protected void Update()
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

    public void SpawnAs(string actorID)
    {
        Actor = null;
        SpawnPlayerObjectServerRpc(OwnerClientId, actorID);
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

        Actor.OnCreatedByPlayerController(this);

        if (IsOwner)
        {
            CameraRig.AttachToActor(Actor);
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

            if (CameraRig.CameraMode == CameraMode.IsometricCamera) // TODO: Dev Check - Remove this block if we remove isometric camera
            {
                Actor.SetForward(moveVec); // TEMP UNTIL 3RD PERSON CAMERA
            }
        }

        if(CameraRig.CameraMode == CameraMode.IsometricCamera) // TODO: Dev Check - Remove this block if we remove isometric camera
        {
            Actor.SetOrientatedMovementVector(new Vector3(0, 0, magnitude)); // TEMP UNTIL 3RD PERSON CAMERA
        }
        else
        {
            Actor.SetOrientatedMovementVector(moveVec);
        }
    }

    public void OnLook(InputValue inputValue)
    {
        if (Actor != null && CameraRig.CameraMode != CameraMode.IsometricCamera)
        {
            Vector2 value = inputValue.Get<Vector2>();

            Actor.AddLookVectorFromInput(value);
        }
        else if(CameraRig.CameraMode == CameraMode.SpectatorCamera)
        {
            // TODO
        }
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
        else if(CameraRig != null && inputValue.isPressed)
        {
            CameraRig.SetToNextVantageCamera();
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

    public void OnPause(InputValue inputValue)
    {
        // TODO: Open the pause menu
    }

    public void OnChangeCharacter(InputValue inputValue)
    {
        if (inputValue.isPressed)
        {
            characterSelectUI.gameObject.SetActive(true);
            characterSelectUI.transform.SetSiblingIndex(transform.childCount);
        }
    }

    #endregion Controls Events

[ServerRpc]
    public void SpawnPlayerObjectServerRpc(ulong clientID, string actorID)
    {
        ActorDefinition actorDef = gameDatabase.availableCharacters.FirstOrDefault(a => a.internalID.Equals(actorID, StringComparison.OrdinalIgnoreCase));

        if(actorDef == null)
        {
            Debug.LogError($"Could not spawn character with ID {actorID} - Actor could not be found in the internal database.");
            return;
        }

        if (actorDef.prefab == null)
        {
            Debug.LogError($"Could not spawn character with ID {actorID} - Actor prefab in definition was null.");
            return;
        }

        if(Actor != null)
        {
            Actor.NetworkObject.Despawn(true);
            Actor = null;
        }

        BaseActor ba = Instantiate(actorDef.prefab, transform);
        ba.GetComponent<NetworkObject>().SpawnWithOwnership(clientID);
        //ba.GetComponent<NetworkObject>().Spawn();
    }
}
