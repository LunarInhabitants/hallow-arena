using MLAPI.NetworkVariable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class BaseActor
{
    #region Networked Variables

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

    #endregion Networked Variables

    protected virtual void PreNetworkUpdate()
    {
        if(!IsOwner)
        {  
            // TODO: Add interp to the actor
            timeSinceLastPositionSync += Time.deltaTime;
            if (timeSinceLastPositionSync > 0.2f)
            {
                Teleport(Position.Value, Rotation.Value);
                timeSinceLastPositionSync = 0.0f;
            }
        }
    }

    protected virtual void PostNetworkUpdate()
    {
        if (IsOwner)
        {
            SetPosition(transform.position);
            SetRotation(transform.rotation);
        }
    }
}
