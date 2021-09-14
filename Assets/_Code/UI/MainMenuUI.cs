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

    [SerializeField] private TMP_InputField playerNameInput;
    [SerializeField] private TMP_Dropdown levelSelectDropdown;
    [SerializeField] private TMP_InputField serverIpInput;

    private MapDefinition selectedMap;

    protected void Awake()
    {
        playerNameInput.text = $"{System.Environment.UserName}#{Random.Range(0, 9999):0000}";

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
