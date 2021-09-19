using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum CameraMode
{
    /// <summary>The default player camera mode.</summary>
    PlayerCamera,
    /// <summary><see cref="PlayerCamera"/> with freelook.</summary>
    SpectatorCamera,
    /// <summary>A static vantage camera from a map vantage point.</summary>
    MapVantageSpectatorCamera,
    /// <summary>A full free camera, unbound. TODO: Needs to accept input in some way.</summary>
    FreeCamera,
    /// <summary>[DEV] The original isometric camera mode.</summary>
    IsometricCamera,
}

public class PlayerCameraRig : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private Transform followTarget;
    [SerializeField] private Transform lookAtTarget;

    public CameraMode CameraMode { get; private set; } = CameraMode.PlayerCamera;
    public MapVantageCamera CurrentMapVantageCamera { get; private set; }

    private BaseActor myActor;

    protected void Update()
    {
        if((CameraMode == CameraMode.PlayerCamera || CameraMode == CameraMode.SpectatorCamera) && myActor != null)
        {
            followTarget.position = myActor.transform.position + new Vector3(0.0f, 2.25f, 0.0f);
            followTarget.rotation = Quaternion.LookRotation(myActor.LookDirection);
            lookAtTarget.position = followTarget.position + new Vector3(0.0f, 0.1f, 0.0f);
        }
    }

    /// <summary>
    /// Attaches the camera rig to a given actor, either as the active player or a spectator.<para/>
    /// Sets <see cref="CameraMode"/> to <see cref="CameraMode.PlayerCamera"/> or <see cref="CameraMode.SpectatorCamera"/> based on <paramref name="asSpectator"/>.
    /// </summary>
    /// <param name="actor">The actor to attach to.</param>
    /// <param name="asSpectator">If true, the camera is a spectator camera, allowing freelook.</param>
    /// <param name="asIsometric">[DEV] If true, the camera is set to an isometric one. This may be stripped in the future.</param>
    public void AttachToActor(BaseActor actor, bool asSpectator = false, bool asIsometric = false)
    {
        myActor = actor;

        if (!asIsometric) // TODO: Dev check. If isometric camera gets removed, remove this if statement and the entire else block.
        {
            CameraMode = asSpectator ? CameraMode.SpectatorCamera : CameraMode.PlayerCamera;
        }
        else
        {
            CameraMode = CameraMode.IsometricCamera; 
        }

        virtualCamera.enabled = true;
        myActor.OnCameraRigAttached(this);

        if(CurrentMapVantageCamera != null)
        {
            CurrentMapVantageCamera.Deactivate();
            CurrentMapVantageCamera = null;
        }
    }

    #region Map Vantage Cameras

    public void SetMapVantageCamera(MapVantageCamera vantageCamera)
    {
        if (vantageCamera == null)
        {
            if (MapController.Instance == null)
            {
                Debug.LogError("Could not set a map vantage camera. The current MapController.Instance is null!");
                return;
            }

            vantageCamera = MapController.Instance.VantageCameras.FirstOrDefault();

            if (vantageCamera == null)
            {
                Debug.LogError("Could not set a map vantage camera. The current MapController.Instance doesn't have any vantage cameras set!");
                return;
            }
        }

        if (CurrentMapVantageCamera == vantageCamera)
        {
            // No need to do anything
            return; 
        }
        else if (CurrentMapVantageCamera != null)
        {
            CurrentMapVantageCamera.Deactivate();
        }

        CurrentMapVantageCamera = vantageCamera;
        CurrentMapVantageCamera.Activate();
        CameraMode = CameraMode.MapVantageSpectatorCamera;
        virtualCamera.enabled = false;
    }

    public void SetToNextVantageCamera()
    {
        if (MapController.Instance == null)
        {
            Debug.LogError("Could not get next vantage camera. The current MapController.Instance is null!");
            return;
        }

        if(CurrentMapVantageCamera == null)
        {
            // This will select the first available camera - if possible.
            SetMapVantageCamera(null);
            return;
        }

        var availableCameras = MapController.Instance.VantageCameras;
        int idx = availableCameras.IndexOf(CurrentMapVantageCamera) + 1;

        if (idx >= availableCameras.Count)
        {
            idx = 0;
        }

        SetMapVantageCamera(availableCameras[idx]);
    }

    public void SetToPreviousVantageCamera()
    {
        if (MapController.Instance == null)
        {
            Debug.LogError("Could not get next vantage camera. The current MapController.Instance is null!");
            return;
        }

        if (CurrentMapVantageCamera == null)
        {
            // This will select the first available camera - if possible.
            SetMapVantageCamera(null);
            return;
        }

        var availableCameras = MapController.Instance.VantageCameras;
        int idx = availableCameras.IndexOf(CurrentMapVantageCamera) - 1;

        if (idx < 0)
        {
            idx = Mathf.Max(0, availableCameras.Count - 1);
        }

        SetMapVantageCamera(availableCameras[idx]);
    }

    #endregion Map Vantage Cameras

    public void SetMapFreelookCamera()
    {
        CameraMode = CameraMode.FreeCamera;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(followTarget.position, lookAtTarget.position);
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(followTarget.position, virtualCamera.transform.position);
    }
#endif
}
