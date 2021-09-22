using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI characterNameLbl;
    [SerializeField] private Image characterImage;

    private Button button;
    private ActorDefinition myActor;
    private CharacterSelectUI parentUI;

    public void Init(ActorDefinition def, CharacterSelectUI ui)
    {
        parentUI = ui;

        myActor = def;
        characterNameLbl.text = def.displayName;
        characterImage.sprite = def.icon;

        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        parentUI.OnCharacterClick(myActor);
    }
}
