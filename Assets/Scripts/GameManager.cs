using Dinojump.Schemas;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    LobbyUIHandler lobbyUIHandler;
    SpawnManager spawnManager;

    [SerializeField]
    public GameObject playerPrefab;

    public Dictionary<string, PlayerSchema> playerList = new Dictionary<string, PlayerSchema>();

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
    public async void EnterLobby(string playerName)
    {
        SceneManager.UnloadSceneAsync("Menu");
        StartCoroutine(AwaitGameScene());

        await RoomManager.Instance.ConnectLobby(playerName);

        lobbyUIHandler = GameObject.Find("LobbyUI").GetComponent<LobbyUIHandler>();
        spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();

        if (RoomManager.Instance.colyseusRoom != null)
        {
            lobbyUIHandler.SetLobbyCode(RoomManager.Instance.colyseusRoom.RoomId);
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

    internal void OnGameStepChange(string currentValue, string previousValue)
    {
        switch (currentValue)
        {
            default:
                //ShowLobbyUI
                GameObject.Find("LobbyUI").SetActive(false);
                break;
            case "Starting":
                GameObject.Find("CountdownUI").SetActive(true);
                StartCoroutine("CountDown");
                    break;
            case "Ongoing":
                //Hide Countdown
                break;
            case "Ended":
                //Show Scoreboard
                break;


        }
    }

    IEnumerator CountDown()
    {
        int i = 0;
        while (i < 5)
        {
            var countdownValue = 5 - i;
            var text = GameObject.Find("CountdownUI").GetComponent<UIDocument>().rootVisualElement.Q<Label>("countdown-text");
            text.text = countdownValue.ToString();
            i++;
            yield return new WaitForSeconds(1);
            
            GameObject.Find("CountdownUI").SetActive(false);
        }
        
        
    }

    

    internal void OnPlatformAdd(string key, PlatformSchema value)
    {
        spawnManager.SpawnPlatform(key, value);
    }

    internal void OnPlatformChange(string key, PlatformSchema value)
    {
        spawnManager.UpdatePlatform(key, value);
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
            lobbyUIHandler.SetLobbyCode(RoomManager.Instance.colyseusRoom.RoomId);
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
        var player = Instantiate(playerPrefab);
        player.transform.Find("Arrow").gameObject.SetActive(key == myPlayerKey);
        var playerController = player.GetComponent<PlayerController>();
        playerController.playerSchema = playerSchema;

        playerSchema.OnChange(() => {
            Debug.Log(playerSchema);
            playerController.playerSchema = playerSchema;
        });

        playerSchema.OnIsReadyChange((current, previous) => {
            Debug.Log("ON IS READY CHANGE");
            Debug.Log(current);
        });
        /*
        playerSchema.position.OnChange(() => {
            Debug.Log("POSTITION CHANGE");
            Debug.Log(playerSchema.position.x);
        });
        */
    }
    internal void OnPlayerChange(string key, PlayerSchema playerSchema)
    {
        Debug.Log("player change");
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

    enum GameStep
    {
        LOBBY = 0,
        STARTING = 1,
        ONGOING = 2,
        ENDED = 3
    }

    #endregion
}
