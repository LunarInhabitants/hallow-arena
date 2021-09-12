using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A general informational object used by systems to list all available characters.<para/>
/// This would, for example, but used in character selection.
/// </summary>
public class ActorDefinition : ScriptableObject
{
    public string displayName;
    public Sprite icon;
    public BaseActor prefab;
}
