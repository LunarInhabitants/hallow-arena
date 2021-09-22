using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectUI : MonoBehaviour
{
    [SerializeField] private GameDatabase gameDatabase;
    [SerializeField] private CharacterSelectButton characterSelectButtonPrefab;
    [SerializeField] private RectTransform characterSelectButtonContainer;

    protected void Start()
    {
        foreach(var character in gameDatabase.availableCharacters)
        {
            CharacterSelectButton csb = Instantiate(characterSelectButtonPrefab, characterSelectButtonContainer);
            csb.Init(character, this);
        }
    }

    public void OnCharacterClick(ActorDefinition def)
    {
        PlayerController.LocalPlayerController.SpawnAs(def.internalID);
        gameObject.SetActive(false);
    }
}
