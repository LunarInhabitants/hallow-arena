using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// This is the actor-specific UI, detailing things like their health, ability states and whatnot.<para/>
/// Game mode specific UI should be in an appropiate game mode UI class.
/// </summary>
public class ActorUI : MonoBehaviour
{
    public TextMeshProUGUI healthTextBox;
    public RectTransform healthFilledBar;

    private int maxHealth = -1;
    private int lastHealth = -1;
    private float healthInterpValue;

    protected void Update()
    {
        healthInterpValue = Mathf.Lerp(healthInterpValue, lastHealth, 0.2f);

        Vector2 newSize = healthFilledBar.localScale;
        newSize.x = Mathf.Clamp01(healthInterpValue / maxHealth);
        healthFilledBar.localScale = newSize;

        healthTextBox.color = Color.Lerp(healthTextBox.color, Color.white, 0.05f);
    }

    public void Init(int initialHealth)
    {
        maxHealth = initialHealth;
        SetCurrentHealth(initialHealth, false);
    }

    public void SetCurrentHealth(int newHealth, bool playUIEffects = true)
    {
        healthTextBox.text = newHealth.ToString();

        if(playUIEffects)
        {
            if (newHealth < lastHealth) // We just got hurt
            {
                healthTextBox.color = Color.red;
            }
            else if(newHealth > lastHealth) // We just got healed
            {
                healthTextBox.color = Color.green;
            }
        }
        else
        {
            healthInterpValue = newHealth;
        }

        lastHealth = newHealth;
    }
}

