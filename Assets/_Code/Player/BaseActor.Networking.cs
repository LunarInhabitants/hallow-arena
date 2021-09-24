using MLAPI;
using MLAPI.NetworkVariable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class BaseActor
{
    #region Networked Variables

    public NetworkVariableVector3 _Net_Position = new NetworkVariableVector3(new NetworkVariableSettings
    {
        WritePermission = NetworkVariablePermission.ServerOnly,
        ReadPermission = NetworkVariablePermission.Everyone
    });

    public NetworkVariableVector3 _Net_LookDirection = new NetworkVariableVector3(new NetworkVariableSettings
    {
        WritePermission = NetworkVariablePermission.ServerOnly,
        ReadPermission = NetworkVariablePermission.Everyone
    });

    private float timeSinceLastPositionSync = 0.0f;

    #endregion Networked Variables

    protected virtual void PreNetworkUpdate()
    {
        if(!IsOwner)
        {  
            // TODO: Add interp to the actor
            timeSinceLastPositionSync += Time.deltaTime;
            if (timeSinceLastPositionSync > 0.2f)
            {
                transform.position = _Net_Position.Value;
                LookDirection = _Net_LookDirection.Value;
                timeSinceLastPositionSync = 0.0f;
            }
        }
    }

    protected virtual void PostNetworkUpdate()
    {
        if (IsOwner)
        {
            SetPosition(transform.position);
            SetLookDirection(LookDirection);
        }
    }
}
