using Fusion;
using Fusion.Sockets;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ConnManager : Fusion.Behaviour, INetworkRunnerCallbacks
{
    public static ConnManager instance;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        Screen.SetResolution(800, 600, FullScreenMode.Windowed);

        runner = GetComponent<NetworkRunner>();
        runner.ProvideInput = true;
    }

    NetworkRunner runner;

    public string userNickname;

    async void StartGame(GameMode mode, string sessionName)
    {
        userNickname = "Player" + UnityEngine.Random.Range(0, 1000000);

        SceneRef scene = SceneRef.FromIndex(1);
        NetworkSceneInfo sceneInfo = new NetworkSceneInfo();
        sceneInfo.AddSceneRef(scene, LoadSceneMode.Single);
        
        await runner.StartGame(new StartGameArgs()
        {
            GameMode = mode,
            SessionName = sessionName,
            Scene = scene,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });
    }

    Dictionary<PlayerRef, NetworkObject> spawnedPlayerList = new Dictionary<PlayerRef, NetworkObject>();
    public GameObject playerFactory;
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        // 서버일때
        if (runner.IsServer)
        {
            // 주인공을 생성하고싶다.
            Vector3 point = UnityEngine.Random.insideUnitSphere * 5f;
            point.y = 0;
            NetworkObject netObj = runner.Spawn(playerFactory, point, Quaternion.identity, player);

            spawnedPlayerList.Add(player, netObj);
        }
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        // spawnedPlayerList에서 player을 찾아서 그녀석의 NetworkObject를 파괴하고싶다.
        if (spawnedPlayerList.TryGetValue(player, out NetworkObject netObj))
        {
            runner.Despawn(netObj);
            spawnedPlayerList.Remove(player);
        }
    }

    void Update()
    {

    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        NetworkInputData data = new NetworkInputData();

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        data.mouseX = Input.GetAxis("Mouse X");

        data.direction = new Vector3(h, 0, v);

        input.Set(data);
    }


    public void OnConnectedToServer(NetworkRunner runner)
    {
        
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
        
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
        
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
        
    }

    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {
        
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
        
    }


    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
        
    }

    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        
    }

    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        
    }


    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {
        
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
    {
        
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
        
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
        
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
        
    }


}
