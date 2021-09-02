using MLAPI;
using MLAPI.Transports.UNET;
using UnityEngine;

public class Dbg_ConnectionUI : MonoBehaviour
{
    public string targetIP = "127.0.0.1";
    public int targetPort = 7777;

    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, 300, 300));
        if (!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer)
        {
            StartButtons();
        }
        else
        {
            StatusLabels();
        }

        GUILayout.EndArea();
    }

    void StartButtons()
    {
        if (GUILayout.Button("Host Server"))
        {
            NetworkManager.Singleton.StartHost();
        }

        GUILayout.Space(4.0f);
        targetIP = GUILayout.TextField(targetIP);
        string newPort = GUILayout.TextField(targetPort.ToString());
        if(int.TryParse(newPort, out int newTargetPort))
        {
            targetPort = newTargetPort;
        }

        if (GUILayout.Button("Connect"))
        {
            UNetTransport transport = NetworkManager.Singleton.GetComponent<UNetTransport>();
            transport.ConnectAddress = targetIP;
            transport.ConnectPort = targetPort;
            NetworkManager.Singleton.StartClient();
        }

        if (GUILayout.Button("Run As Dedicated Server"))
        {
            NetworkManager.Singleton.StartServer();
        }
    }

    static void StatusLabels()
    {
        var mode = NetworkManager.Singleton.IsHost ? "Host" : NetworkManager.Singleton.IsServer ? "Server" : "Client";

        if(GUILayout.Button("Disconnect"))
        {
            if(NetworkManager.Singleton.IsHost)
            {
                NetworkManager.Singleton.StopHost();
            }
            else if (NetworkManager.Singleton.IsClient)
            {
                NetworkManager.Singleton.StopClient();
            }
            else if (NetworkManager.Singleton.IsServer)
            {
                NetworkManager.Singleton.StopServer();
            }
        }


        GUILayout.Label("Transport: " + NetworkManager.Singleton.NetworkConfig.NetworkTransport.GetType().Name);
        GUILayout.Label("Mode: " + mode);
    }
}
