using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A static vantage point used during spectation modes. Should show off a key part of the map.
/// </summary>
[RequireComponent(typeof(CinemachineVirtualCamera))]
public class MapVantageCamera : MonoBehaviour
{
    private CinemachineVirtualCamera virtualCamera;

    protected void Start()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        Deactivate();
    }

    public void Activate()
    {
        virtualCamera.enabled = true;
    }

    public void Deactivate()
    {
        virtualCamera.enabled = false;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.color = Color.green;
        Gizmos.DrawFrustum(Vector3.zero, 40.0f, 32.0f, 0.5f, 16.0f / 9.0f);
    }
#endif
}
