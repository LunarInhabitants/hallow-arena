using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerNameplate : MonoBehaviour
{
    [SerializeField] private CanvasGroup nameplateLabelContainer;
    [SerializeField] private TextMeshProUGUI nameplateLabel;
    [SerializeField] private float minFadeoffDist = 16.0f;
    [SerializeField] private float maxFadeoffDist = 20.0f;

    protected void Update()
    {
        Transform cameraTransform = Camera.main.transform;
        float cameraDist = (transform.position - cameraTransform.position).magnitude;

        // Always look at the camera
        transform.forward = cameraTransform.forward;
        // Fade out at distance
        nameplateLabelContainer.alpha = Mathf.InverseLerp(maxFadeoffDist, minFadeoffDist, cameraDist);
    }

    public void SetName(string newName)
    {
        nameplateLabel.text = newName;
    }

    public void SetTeam(ETeam team)
    {
        // TODO: Determine team colours. Maybe store them in the GameDatabase for accessibility support?
        switch(team)
        {
            default:
            case ETeam.NoTeam:
                nameplateLabel.color = Color.white;
                break;

            case ETeam.Team1:
                nameplateLabel.color = Color.red;
                break;

            case ETeam.Team2:
                nameplateLabel.color = Color.blue;
                break;
        }
    }
}
