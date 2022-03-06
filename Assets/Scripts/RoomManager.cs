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
        
        // colyseusClient = new ColyseusClient("wss://dinojump-server.herokuapp.com");
         colyseusClient = new ColyseusClient("ws://localhost:3002");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public async Task ConnectLobby(string playerName, string code = null)
    {
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
        colyseusRoom.State.platforms.OnAdd(GameManager.Instance.OnPlatformAdd);
        colyseusRoom.State.platforms.OnChange(GameManager.Instance.OnPlatformChange);
        colyseusRoom.State.platforms.OnRemove(GameManager.Instance.OnPlatformRemove);

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
}
