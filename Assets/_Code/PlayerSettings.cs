using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSettings : MonoBehaviour
{
    private static PlayerSettings _instance = null;

    const string PREF_PREFIX = "PlayerSettings";

    const string DEFAULTNAME = "Player";
    private string playerName_Pref = $"{PREF_PREFIX}.Name";
    private string playerName;

    void Awake()
    {
        if (_instance != null) 
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(gameObject);

        Init();
    }

    void Init()
    {
        playerName = PlayerPrefs.GetString(playerName_Pref, DEFAULTNAME);
    }

    public string GetName()
    {
        return playerName;
    }

    public bool SetName(string newName)
    {
        if (newName == playerName) { return true; }
        if (!VerifyName(newName)) { return false; }

        playerName = newName;
        PlayerPrefs.SetString(playerName_Pref, playerName);
        Debug.Log("New name set: " + playerName);
        return true;
    }

    public bool VerifyName(string nameToCheck)
    {
        return true;
    }
}
