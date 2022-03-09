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
    SpawnManager spawnManager;

    [SerializeField]
    public GameObject playerPrefab;

    public Dictionary<string, PlayerSchema> playerList;

    public string myPlayerKey;
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
        spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();

        if (RoomManager.Instance.colyseusRoom != null)
        {
            lobbyUIHandler.SetLobbyCode(RoomManager.Instance.colyseusRoom.Id);
        }
    }


    IEnumerator AwaitGameScene()
    {
        var sceneLoad = SceneManager.LoadSceneAsync("Game", LoadSceneMode.Additive);
        while (!sceneLoad.isDone)
        {
            yield return null;
        }
    }

    internal void OnPlatformAdd(string key, PlatformSchema value)
    {
        spawnManager.SpawnPlatform(key, value);
    }

    internal void OnPlatformChange(string key, PlatformSchema value)
    {
        spawnManager.UpdatePlatform(key,value);
    }

    internal void OnPlatformRemove(string key, PlatformSchema value)
    {
        spawnManager.RemovePlatform(key);
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
    #region PlayerListeners
    public void OnPlayerAdd(string key, PlayerSchema playerSchema)
    {
        Debug.Log("Player Added, Key: " + key);

        playerList.Add(key, playerSchema);
        RefreshLobbyUI();
        var newPlayer = Instantiate(playerPrefab);
        newPlayer.transform.Find("Arrow").gameObject.SetActive(key == myPlayerKey);
        newPlayer.GetComponent<PlayerController>().playerSchema = playerSchema;

    }
    internal void OnPlayerChange(string key, PlayerSchema playerSchema)
    {
        playerList[key] = playerSchema;
        lobbyUIHandler.RenderPlayerNames();
    }
    public void OnPlayerRemove(string key, PlayerSchema playerSchema)
    {
        playerList.Remove(key);
        Destroy(FindObjectsOfType<PlayerController>().FirstOrDefault(p => p.playerSchema.sessionId == key).gameObject);
        RefreshLobbyUI();
    }

    void RefreshLobbyUI()
    {
        if (lobbyUIHandler != null)
        {
            lobbyUIHandler.RenderPlayerNames();
        }
    }

    #endregion
}
