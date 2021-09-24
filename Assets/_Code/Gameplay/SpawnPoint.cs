using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [field: SerializeField] public ETeam Team { get; private set; }

    public float TimeSinceLastRespawn { get; private set; }

    private bool hasRegisteredSpawnPoint = false;

    protected void Start()
    {
        if (BaseGameMode.Instance != null)
        {
            BaseGameMode.Instance.RegisterSpawnPoint(this);
            hasRegisteredSpawnPoint = true;
        }
    }

    protected void Update()
    {
        if(!hasRegisteredSpawnPoint && BaseGameMode.Instance != null)
        {
            BaseGameMode.Instance.RegisterSpawnPoint(this);
            hasRegisteredSpawnPoint = true;
        }

        // Note - we clamp it to 20 since a spawn that hasn't been used in over 20 seconds is already very stale.
        TimeSinceLastRespawn = Mathf.Min(20.0f, TimeSinceLastRespawn + Time.deltaTime);
    }

    /// <summary>
    /// Spawn the given actor on this spawn point.<para/>
    /// NOTE: THIS SHOULD ONLY BE CALLED BY THE SERVER!
    /// </summary>
    /// <param name="actor">The actor to spawn.</param>
    public void Spawn(BaseActor actor)
    {
        Debug.Log($"Spawning {actor} at {transform.position}");
        actor.Teleport(transform.position, transform.forward);
        TimeSinceLastRespawn = 0.0f;
    }

#if UNITY_EDITOR
    protected void OnDrawGizmos()
    {
        Gizmos.matrix = transform.localToWorldMatrix;
        switch(Team)
        {
            default:
                Gizmos.color = Color.magenta * 0.5f;
                break;

            case ETeam.NoTeam:
                Gizmos.color = Color.white * 0.5f;
                break;

            case ETeam.Team1:
                Gizmos.color = Color.red * 0.5f;
                break;

            case ETeam.Team2:
                Gizmos.color = Color.blue * 0.5f;
                break;
        }

        AdvGizmos.DrawHumanoid(Vector3.zero);
    }
#endif
}
