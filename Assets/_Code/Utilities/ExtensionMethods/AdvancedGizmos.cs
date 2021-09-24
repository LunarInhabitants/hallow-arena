using UnityEngine;

public static class AdvGizmos
{
    private static readonly Vector3 groinOffset = new Vector3(0.0f, 0.49f, 0.0f);
    private static readonly Vector3 groinSize = new Vector3(0.18f, 0.08f, 0.1f);

    private static readonly Vector3 lowerTorsoOffset = new Vector3(0.0f, 0.6f, 0.0f);
    private static readonly Vector3 lowerTorsoSize = new Vector3(0.18f, 0.1f, 0.1f);

    private static readonly Vector3 upperTorsoOffset = new Vector3(0.0f, 0.74f, 0.0f);
    private static readonly Vector3 upperTorsoSize = new Vector3(0.18f, 0.16f, 0.1f);

    private static readonly Vector3 neckOffset = new Vector3(0.0f, 0.845f, -0.015f);
    private static readonly Vector3 neckSize = new Vector3(0.07f, 0.036f, 0.07f);

    private static readonly Vector3 headOffset = new Vector3(0.0f, 0.925f, 0.0f);
    private static readonly Vector3 headSize = new Vector3(0.1f, 0.11f, 0.1f);

    private static readonly Vector3 noseOffest = new Vector3(0.0f, 0.92f, 0.06f);
    private static readonly Vector3 noseSize = new Vector3(0.015f, 0.025f, 0.015f);

    private static readonly Vector3 lUpperArmOffset = new Vector3(-0.14f, 0.74f, 0.0f);
    private static readonly Vector3 rUpperArmOffset = new Vector3(0.14f, 0.74f, 0.0f);
    private static readonly Vector3 upperArmSize = new Vector3(0.08f, 0.16f, 0.1f);

    private static readonly Vector3 lLowerArmOffset = new Vector3(-0.14f, 0.57f, 0.0f);
    private static readonly Vector3 rLowerArmOffset = new Vector3(0.14f, 0.57f, 0.0f);
    private static readonly Vector3 lowerArmSize = new Vector3(0.08f, 0.16f, 0.1f);

    private static readonly Vector3 lHandOffset = new Vector3(-0.14f, 0.43f, 0.0f);
    private static readonly Vector3 rHandOffset = new Vector3(0.14f, 0.43f, 0.0f);
    private static readonly Vector3 handSize = new Vector3(0.04f, 0.1f, 0.08f);

    private static readonly Vector3 lUpperLegOffset = new Vector3(-0.05f, 0.36f, 0.0f);
    private static readonly Vector3 rUpperLegOffset = new Vector3(0.05f, 0.36f, 0.0f);
    private static readonly Vector3 upperLegSize = new Vector3(0.08f, 0.16f, 0.1f);

    private static readonly Vector3 lLowerLegOffset = new Vector3(-0.05f, 0.17f, 0.0f);
    private static readonly Vector3 rLowerLegOffset = new Vector3(0.05f, 0.17f, 0.0f);
    private static readonly Vector3 lowerLegSize = new Vector3(0.08f, 0.2f, 0.1f);

    private static readonly Vector3 lFootOffset = new Vector3(-0.05f, 0.03f, 0.04f);
    private static readonly Vector3 rFootOffset = new Vector3(0.05f, 0.03f, 0.04f);
    private static readonly Vector3 footSize = new Vector3(0.08f, 0.06f, 0.18f);

    public static void DrawHumanoid(Vector3 position, float height = 1.8f)
    {
        Matrix4x4 oldMatrix = Gizmos.matrix;
        Gizmos.matrix *= Matrix4x4.Translate(position);
        Gizmos.matrix *= Matrix4x4.Scale(new Vector3(height, height, height));

        Gizmos.DrawCube(groinOffset, groinSize);
        Gizmos.DrawCube(lowerTorsoOffset, lowerTorsoSize);
        Gizmos.DrawCube(upperTorsoOffset, upperTorsoSize);
        Gizmos.DrawCube(neckOffset, neckSize);
        Gizmos.DrawCube(headOffset, headSize);
        Gizmos.DrawCube(noseOffest, noseSize);

        Gizmos.DrawCube(lUpperArmOffset, upperArmSize);
        Gizmos.DrawCube(lLowerArmOffset, lowerArmSize);
        Gizmos.DrawCube(lHandOffset, handSize);

        Gizmos.DrawCube(rUpperArmOffset, upperArmSize);
        Gizmos.DrawCube(rLowerArmOffset, lowerArmSize);
        Gizmos.DrawCube(rHandOffset, handSize);

        Gizmos.DrawCube(lUpperLegOffset, upperLegSize);
        Gizmos.DrawCube(lLowerLegOffset, lowerLegSize);
        Gizmos.DrawCube(lFootOffset, footSize);

        Gizmos.DrawCube(rUpperLegOffset, upperLegSize);
        Gizmos.DrawCube(rLowerLegOffset, lowerLegSize);
        Gizmos.DrawCube(rFootOffset, footSize);

        Gizmos.matrix = oldMatrix;
    }

    public static void DrawGrip()
    {
        Matrix4x4 oldMat = Gizmos.matrix;
        Gizmos.DrawCube(new Vector3(0.0f, 0.065f, 0.065f), new Vector3(0.02f, 0.03f, 0.17f));

        Gizmos.matrix *= Matrix4x4.Rotate(Quaternion.Euler(15.0f, 0.0f, 0.0f));
        Gizmos.DrawCube(new Vector3(0.0f, 0.01f, 0.0f), new Vector3(0.02f, 0.1f, 0.035f));

        Gizmos.matrix = oldMat;
    }
}
