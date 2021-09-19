using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Holds global state about a map. Gamemode agnostic, although you can access the current gamemode from here.
/// </summary>
public class MapController : MonoBehaviour
{
    public static MapController Instance { get; private set; }

    [field: SerializeField] public List<MapVantageCamera> VantageCameras { get; private set; } = new List<MapVantageCamera>();

    private void Awake()
    {
        Instance = this;
    }
}
