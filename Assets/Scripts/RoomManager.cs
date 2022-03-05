using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Colyseus;
using Dinojump.Schemas;
public class RoomManager : MonoBehaviour
{
    private ColyseusClient colyseusClient;
    private ColyseusRoom<GameSchema> colyseusRoom;

    async void ConnectRoom()
    {
        Dictionary<string, object> roomOptions = new Dictionary<string, object>
        {
            ["name"] = "My Super Name"
        };
        colyseusRoom = await colyseusClient.Create<GameSchema>("GameRoom", roomOptions);

        
        // colyseusRoom = await colyseusClient.JoinById<GameSchema>("248A", roomOptions);

        Debug.Log("Thats My SESSION_ID: " + colyseusRoom.SessionId);

        colyseusRoom.State.players.OnAdd += (string key, PlayerSchema playerSchema) =>
        {
            Debug.Log(key);
            // Instantiate PlayerPrefab with key
        };

        colyseusRoom.State.players.OnChange += (string key, PlayerSchema playerSchema) =>
        {
            Debug.Log("Changed" + key);
            // Update PlayerObject in scene with key
        };

        colyseusRoom.State.players.OnRemove += (string key, PlayerSchema playerSchema) =>
        {
            Debug.Log(key);
            // Remove PlayerObject in scene with key
        };
    }
    // Start is called before the first frame update
    void Start()
    {
        colyseusClient = NetworkManager.Instance.CreateClient("wss://" + NetworkManager.Instance.ColyseusServerAddress);
        ConnectRoom();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
