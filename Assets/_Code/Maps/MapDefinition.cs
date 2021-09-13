using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used to store information about a map, for use on level select.
/// </summary>
[CreateAssetMenu(fileName = "MapReference.asset", menuName = "Hallow Arena/Maps/Map Reference", order = 182)]
public class MapDefinition : ScriptableObject
{
    /// <summary>The ID of the scene in the game's build list.</summary>
    public int sceneBuildIndex;
    public string displayName = "A map";
    public string gameMode = "Deathmatch";
    public string description = "A map";
    public Sprite icon;
}
