using Cinemachine;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : NetworkBehaviour
{
    private ActorController actorController;

    public override void NetworkStart()
    {
        actorController = GetComponent<ActorController>();

        actorController.Init(IsLocalPlayer, IsServer);
    }

    void Update()
    {

    }

    void LocalUpdate()
    {

    }

    public void OnMove(InputValue inputValue)
    {
        actorController.Move(inputValue.Get<Vector2>());        
    }

    public void OnJump(InputValue inputValue)
    {
        actorController.Jump();
    }

    public void OnWalk(InputValue inputValue)
    {
        actorController.IsWalking = inputValue.isPressed;
    }

    public void OnEmote1(InputValue inputValue)
    {
       // SetAnimationTrigger("Emote_Wave");
    }

    public void OnEmote2(InputValue inputValue)
    {
        //SetAnimationTrigger("Emote_Point");
    }

    public void OnEmote3(InputValue inputValue)
    {
       // SetAnimationTrigger("Emote_Insult");
    }


}
