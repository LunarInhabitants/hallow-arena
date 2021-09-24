using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A global store for commonly needed data, such as info on maps, characters and whatnot.
/// </summary>
[CreateAssetMenu(fileName = "GameDatabase.asset", menuName = "Hallow Arena/Game Database", order = 182)]
public class GameDatabase : ScriptableObject
{
    public List<ActorDefinition> availableCharacters = new List<ActorDefinition>();
    public List<MapDefinition> availableMaps = new List<MapDefinition>();
}
