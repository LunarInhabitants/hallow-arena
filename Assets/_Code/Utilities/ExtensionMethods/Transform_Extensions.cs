using UnityEngine;

public static class Transform_Extensions
{
    /// <summary>
    /// Recursively sets the layer for the given transform and all its children.
    /// </summary>
    /// <param name="transform">The transform to set the layer for.</param>
    /// <param name="layer">The <see cref="LayerMask"/> to set.</param>
    public static void SetLayerRecursively(this Transform transform, LayerMask layer)
    {
        // Layermask values have bit 'n' set, while gameObject.layer wants the actual value of 'n'.

        int bitmask = layer.value;
        int layerNumber = bitmask > 0 ? 0 : 31;
        while (bitmask > 1)
        {
            bitmask >>= 1;
            layerNumber++;
        }

        SetLayerRecursively(transform, layerNumber);
    }

    /// <summary>
    /// Recursively sets the layer for the given transform and all its children.
    /// </summary>
    /// <param name="transform">The transform to set the layer for.</param>
    /// <param name="layerNumber">The layer number/index to set (This is a value between 0 and 31 inclusive, and is NOT <see cref="LayerMask.value"/>).</param>
    public static void SetLayerRecursively(this Transform transform, int layerNumber)
    {
        transform.gameObject.layer = layerNumber;

        for (int i = 0, count = transform.childCount; i < count; i++)
        {
            transform.GetChild(i).SetLayerRecursively(layerNumber);
        }
    }
}
