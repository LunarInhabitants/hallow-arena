using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CameraExtensions
{
    public static void MoveToAndLookAt(this CinemachineVirtualCamera cam, Vector3 targetPosition, Vector3 lookAt)
    {
        cam.MoveToAndLookAtLerp(targetPosition, lookAt, 1.0f);
    }

    public static void MoveToAndLookAtLerp(this CinemachineVirtualCamera cam, Vector3 targetPosition, Vector3 lookAt, float lerp)
    {
        cam.transform.position = Vector3.Lerp(cam.transform.position, targetPosition, lerp);
        cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, Quaternion.LookRotation(lookAt - cam.transform.position, Vector3.up), lerp);
    }
}