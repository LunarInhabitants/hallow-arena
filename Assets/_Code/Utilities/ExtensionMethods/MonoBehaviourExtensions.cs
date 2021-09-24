using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MonoBehaviourExtensions
{
    /// <summary>
    /// This transforms the supplied vector to based on the object it's applied on.
    /// </summary>
    /// <param name="mb">The monobehaviour the vector will be offset from.</param>
    /// <param name="offsetFromTransform">The vector offset from the object.</param>
    /// <returns></returns>
    public static Vector3 TransformVectorToOffset(this MonoBehaviour mb, Vector3 offsetFromTransform)
    {
        return mb.transform.position + (mb.transform.rotation * offsetFromTransform);
    }
}
