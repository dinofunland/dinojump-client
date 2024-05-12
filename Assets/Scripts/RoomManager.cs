using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Colyseus;
using Dinojump.Schemas;
using System.Threading.Tasks;
using System;
using Dinojump;

public class RoomManager : MonoBehaviour
{
    private ColyseusClient colyseusClient;
    public ColyseusRoom<GameSchema> colyseusRoom;

    public GameObject UIConnectingToServer;
    private bool IsConnecting = false;

    public static RoomManager Instance;
 
    // Start is called before the first frame update
    void Start()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        this.UIConnectingToServer.SetActive(false);
    }

    void CreateClient()
    {
        colyseusClient = NetworkManager.Instance.CreateClient("wss://" + "dinojump-server.pibern.ch");
        // colyseusClient = NetworkManager.Instance.CreateClient("ws://" + "localhost:3001");
    }

    public async Task ConnectLobby(string playerName, string code = null)
    {
        if (IsConnecting) return;
        this.UIConnectingToServer.SetActive(true);
        CreateClient();

        IsConnecting = true;
        Dictionary<string, object> roomOptions = new Dictionary<string, object>
        {
            ["name"] = playerName,
        };
        try
        {
            if (!string.IsNullOrEmpty(code))
                colyseusRoom = await colyseusClient.JoinById<GameSchema>(code, roomOptions);
            else
                colyseusRoom = await colyseusClient.Create<GameSchema>("GameRoom", roomOptions);
        }
        catch (Exception ex)
        {
            this.UIConnectingToServer.SetActive(false);
            IsConnecting = false;
            throw ex;
        }


        colyseusRoom.State.players.OnAdd(GameManager.Instance.OnPlayerAdd);
        colyseusRoom.State.players.OnRemove(GameManager.Instance.OnPlayerRemove);
        colyseusRoom.State.players.OnChange(GameManager.Instance.OnPlayerChange);
        colyseusRoom.State.OnGameStepChange(GameManager.Instance.OnGameStepChange);

        colyseusRoom.State.platforms.OnAdd(GameManager.Instance.OnPlatformAdd);
        colyseusRoom.State.platforms.OnChange(GameManager.Instance.OnPlatformChange);
        colyseusRoom.State.platforms.OnRemove(GameManager.Instance.OnPlatformRemove);

        colyseusRoom.State.OnFloorChange(GameManager.Instance.OnFloorPositionChange);
        colyseusRoom.State.OnScoreChange(GameManager.Instance.OnScoreChange);
        colyseusRoom.OnLeave += GameManager.Instance.OnLeaveLobby;
        colyseusRoom.OnError += GameManager.Instance.OnLobbyError;
        colyseusRoom.OnLeave += OnLeaveLobby;
        colyseusRoom.OnError += OnLobbyError;

        colyseusRoom.OnMessage<EmoteMessage>("emote", GameManager.Instance.OnEmoteMessage);
        GameManager.Instance.myPlayerKey = colyseusRoom.SessionId;

        this.UIConnectingToServer.SetActive(false);
        IsConnecting = false;
    }


    public void Examples(string playerName)
    {
        // colyseusRoom = await colyseusClient.JoinById<GameSchema>("248A", roomOptions);
        Debug.Log("Thats My SESSION_ID: " + colyseusRoom.SessionId);

        colyseusRoom.State.players.OnChange((string key, PlayerSchema playerSchema) =>
        {
            Debug.Log("Changed" + key);
            // Update PlayerObject in scene with key
        });
    }


    private void OnApplicationQuit()
    {
        LeaveRoom();
    }
    private void OnLobbyError(int code, string message)
    {
        LeaveRoom();
    }

    private void OnLeaveLobby(int code)
    {
        LeaveRoom();
    }

    async void LeaveRoom() 
    {
        Debug.Log("Leave");
        await colyseusRoom?.Leave();
        IsConnecting = false;
    }
}
