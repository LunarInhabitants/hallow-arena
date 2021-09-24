using MLAPI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(NetworkObject))]
public abstract class BaseGameMode : NetworkBehaviour
{
    public static BaseGameMode Instance { get; private set; }

    public List<SpawnPoint> SpawnPoints { get; } = new List<SpawnPoint>();

    [field: SerializeField] public string GameModeID { get; private set; }
    [field: SerializeField] public string GameModeName { get; private set; }
    [field: SerializeField] public bool IsTeamGame { get; private set; }
    [SerializeField] private GameModeUI UIPrefab;
    public GameModeUI GameModeUI { get; private set; }

    protected virtual void Awake()
    {
        Instance = this;
        GameModeUI = Instantiate(UIPrefab, transform);
    }

    protected virtual void Start()
    {
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }

    /// <summary>
    /// Registers the spointpoint to the given game mode.
    /// </summary>
    /// <param name="spawnPoint">The spawn point to register.</param>
    public void RegisterSpawnPoint(SpawnPoint spawnPoint)
    {
        SpawnPoints.Add(spawnPoint);
    }

    /// <summary>
    /// Returns the best available spawnpoint for the given team.
    /// </summary>
    /// <param name="team">The team to fetch a best spawn point for.</param>
    /// <returns>The best available spawnpoint for the given team.</returns>
    public SpawnPoint GetBestSpawnPointForRespawn(ETeam team)
    {
        List<SpawnPoint> bestSpawns = new List<SpawnPoint>();
        int bestSpawnWeight = 0;

        foreach(SpawnPoint sp in SpawnPoints.Where(s => s.Team == team))
        {
            int weight = Mathf.CeilToInt(sp.TimeSinceLastRespawn);

            if(weight > bestSpawnWeight)
            {
                // We've got a spawn better than the entire list we previously had.
                // Make a new list containing this spawn.
                bestSpawns = new List<SpawnPoint>() { sp };
                bestSpawnWeight = weight;
            }
            else if(weight == bestSpawnWeight)
            {
                // This spawn is at least as good as an existing one, so add it to the list.
                bestSpawns.Add(sp);
            }
        }

        // Grab a spawn at random - any in this list are all equally 'best'.
        SpawnPoint bestSpawn = bestSpawns.Random();

        if (bestSpawn == null)
        {
            throw new InvalidOperationException($"Could not find a valid spawn for team {team}.");
        }

        return bestSpawn;
    }
}
