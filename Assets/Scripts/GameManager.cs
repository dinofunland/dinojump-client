using Dinojump.Schemas;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    LobbyUIHandler lobbyUIHandler;

    [SerializeField]
    public GameObject playerPrefab;

    public string PlayerName;
    // Start is called before the first frame update
    void Start()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        if (SceneManager.sceneCount == 1)
        {
            SceneManager.LoadScene("Menu", LoadSceneMode.Additive);
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Create New
    public async void EnterLobby(string  playerName)
    {
        SceneManager.UnloadSceneAsync("Menu");
        StartCoroutine(AwaitGameScene());

        await RoomManager.Instance.ConnectLobby(playerName);
        
        lobbyUIHandler = GameObject.Find("LobbyUI").GetComponent<LobbyUIHandler>();
        if (RoomManager.Instance.colyseusRoom != null)
        {
            lobbyUIHandler.SetLobbyCode(RoomManager.Instance.colyseusRoom.Id);
        }
    }

    internal void OnPlayerChange(string key, PlayerSchema playerSchema)
    {
        Debug.Log("Player Changed: " + key);
        lobbyUIHandler.UpdateDictionary(key, playerSchema);
    }

    IEnumerator AwaitGameScene()
    {
        var sceneLoad = SceneManager.LoadSceneAsync("Game", LoadSceneMode.Additive);
        while (!sceneLoad.isDone)
        {
            yield return null;
        }
    }

    //Join Lobby
    public async void EnterLobby(string playerName, string code)
    {
        SceneManager.UnloadSceneAsync("Menu");
        StartCoroutine(AwaitGameScene());

        await RoomManager.Instance.ConnectLobby(playerName, code);

        lobbyUIHandler = GameObject.Find("LobbyUI").GetComponent<LobbyUIHandler>();
        if (RoomManager.Instance.colyseusRoom != null)
        {
            lobbyUIHandler.SetLobbyCode(RoomManager.Instance.colyseusRoom.Id);
        }
    }

    public void ExitGame()
    {
        SceneManager.LoadScene("Main");
    }

    public void OnPlayerAdd(string key, PlayerSchema playerSchema)
    {
        Debug.Log("Player Added, Key: " + key);

        if (lobbyUIHandler != null)
        {
            lobbyUIHandler.AddPlayerToContainer(key, playerSchema);
        }

        var newPlayer = Instantiate(playerPrefab);
        newPlayer.GetComponent<PlayerController>().playerSchema = playerSchema;

    }
    public void OnPlayerRemove(string key, PlayerSchema playerSchema)
    {
        Debug.Log("Player Removed, Key: " + key);

        if (lobbyUIHandler != null)
        {
            lobbyUIHandler.RemovePlayerFromContainer(key);
        }
        Destroy(FindObjectsOfType<PlayerController>().FirstOrDefault(p => p.playerSchema.sessionId == key).gameObject);
    }
}
