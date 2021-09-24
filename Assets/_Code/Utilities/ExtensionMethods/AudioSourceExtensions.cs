using UnityEngine;

public static class AudioSourceExtensions
{
    public static void SetMaxDistWithFalloff(this AudioSource audioSource, float maxDist)
    {
        audioSource.maxDistance = maxDist;
        audioSource.rolloffMode = AudioRolloffMode.Custom;
        AnimationCurve ac = new AnimationCurve();
        for (float i = 0; i < 1.0f; i += 0.1f)
        {
            ac.AddKey(i * maxDist, Mathf.Pow(1.0f / 2048.0f, i));
        }
        ac.AddKey(maxDist, 0.0f);
        audioSource.SetCustomCurve(AudioSourceCurveType.CustomRolloff, ac);
    }
}
