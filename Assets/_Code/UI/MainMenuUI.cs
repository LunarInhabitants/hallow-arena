using MLAPI;
using MLAPI.Transports.UNET;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private GameDatabase gameDatabase;

    private PlayerSettings playerSettings;

    [SerializeField] private TMP_InputField playerNameInput;
    [SerializeField] private TMP_Dropdown levelSelectDropdown;
    [SerializeField] private TMP_InputField serverIpInput;

    private MapDefinition selectedMap;

    protected void Start()
    {
        playerSettings = FindObjectOfType<PlayerSettings>();
        if (playerSettings)
        {
            playerNameInput.text = playerSettings.GetName();
            playerNameInput.onValueChanged.AddListener(OnPlayerNameChanged);
        }
        else
        {
            Debug.LogError("Could not find PlayerSettings");
        }
        

        levelSelectDropdown.onValueChanged.AddListener(OnSelectedLevelChanged);
        levelSelectDropdown.ClearOptions();
        foreach(var map in gameDatabase.availableMaps)
        {
            if(selectedMap == null)
            {
                selectedMap = map;
            }

            levelSelectDropdown.options.Add(new TMP_Dropdown.OptionData
            {
                image = map.icon,
                text = map.displayName,
            });
        }
    }

    private void OnSelectedLevelChanged(int index)
    {
        selectedMap = gameDatabase.availableMaps[index];
    }

    private void OnPlayerNameChanged(string newName)
    {
        playerSettings.SetName(newName);
    }

    public void OnHostServerClick()
    {
        StartCoroutine(DoHostServerClick());
    }

    private IEnumerator DoHostServerClick()
    {
        Debug.Log($"Hosting server on {selectedMap.displayName} as {playerNameInput.text}");
        var load = SceneManager.LoadSceneAsync(selectedMap.sceneBuildIndex, LoadSceneMode.Additive);
        yield return load;
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(selectedMap.sceneBuildIndex));

        Debug.Log($"Starting host...");
        NetworkManager.Singleton.StartHost();

        SceneManager.UnloadSceneAsync(0);
    }

    public void OnConnectToServerClick()
    {
        Debug.Log($"Connecting to server on {serverIpInput.text}:7777 as {playerNameInput.text}");

        UNetTransport transport = NetworkManager.Singleton.GetComponent<UNetTransport>();
        transport.ConnectAddress = serverIpInput.text;
        transport.ConnectPort = 7777;
        NetworkManager.Singleton.StartClient();
    }

    // THIS IS A METHOD FOR DEVELOPMENT
    public void SetQuickIP(string newIP)
    {
        serverIpInput.text = newIP;
    }
}
