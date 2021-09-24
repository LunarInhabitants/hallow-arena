using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A general informational object used by systems to list all available characters.<para/>
/// This would, for example, be used in character selection.
/// </summary>
[CreateAssetMenu(fileName = "ActorDefinition.asset", menuName = "Hallow Arena/Actors/Actor Definition", order = 182)]
public class ActorDefinition : ScriptableObject
{
    public string internalID;
    public string displayName;
    public Sprite icon;
    public BaseActor prefab;
}
