using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Colyseus;
using Dinojump.Schemas;
using System.Threading.Tasks;

public class RoomManager : MonoBehaviour
{
    private ColyseusClient colyseusClient;
    public ColyseusRoom<GameSchema> colyseusRoom;

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
        
        //colyseusClient = NetworkManager.Instance.CreateClient("wss://" + "dinojump-server.herokuapp.com");
        colyseusClient = NetworkManager.Instance.CreateClient("ws://" + "localhost:3000");
    }

    public async Task ConnectLobby(string playerName, string code = null)
    {
        if(IsConnecting) return;

        IsConnecting = true;
        Dictionary<string, object> roomOptions = new Dictionary<string, object>
        {
            ["name"] = playerName,
        };
        if(!string.IsNullOrEmpty(code))
             colyseusRoom = await colyseusClient.JoinById<GameSchema>(code, roomOptions);
        else
            colyseusRoom = await colyseusClient.JoinOrCreate<GameSchema>("GameRoom", roomOptions);
        
        colyseusRoom.State.players.OnAdd(GameManager.Instance.OnPlayerAdd);
        colyseusRoom.State.players.OnRemove(GameManager.Instance.OnPlayerRemove);
        colyseusRoom.State.players.OnChange(GameManager.Instance.OnPlayerChange);
        colyseusRoom.State.OnGameStepChange(GameManager.Instance.OnGameStepChange);

        colyseusRoom.State.platforms.OnAdd(GameManager.Instance.OnPlatformAdd);
        colyseusRoom.State.platforms.OnChange(GameManager.Instance.OnPlatformChange);
        colyseusRoom.State.platforms.OnRemove(GameManager.Instance.OnPlatformRemove);

        colyseusRoom.State.OnFloorChange(GameManager.Instance.OnFloorPositionChange);
        colyseusRoom.State.OnScoreChange(GameManager.Instance.OnScoreChange);
        
        GameManager.Instance.myPlayerKey = colyseusRoom.SessionId;
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

    private async void OnApplicationQuit()
    {
        Debug.Log("Leave");
        await colyseusRoom?.Leave(); 
        IsConnecting = false;
    }
}
