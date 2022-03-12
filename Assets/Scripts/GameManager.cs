using Colyseus.Schema;
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

    [SerializeField]
    private GameObject countdownUI;
    [SerializeField]
    private GameObject gameOverUI;
    [SerializeField]
    private GameObject lobbyUI;

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

    void ConstructGroundAndWalls()
    {
        return;
        Debug.Log("TODO: Cosntruct Ground and Walls");
    }


    internal void OnFloorPositionChange(FloorSchema currentValue, FloorSchema previousValue)
    {
        Debug.Log("Floor Schema changed.");
        var lavaObject = GameObject.Find("LavaContainer").GetComponent<LavaController>();
        lavaObject.floorSchema = currentValue;
    }

    #region LobbyHandling

    //Join Lobby
    public async void ConnectToLobby(string playerName, string code)
    {
        SceneManager.UnloadSceneAsync("Menu");
        StartCoroutine(AwaitGameScene());
        await RoomManager.Instance.ConnectLobby(playerName, code);
    }

    //Create New
    public async void ConnectToLobby(string playerName)
    {
        SceneManager.UnloadSceneAsync("Menu");
        StartCoroutine(AwaitGameScene());
        await RoomManager.Instance.ConnectLobby(playerName);
    }

    void PrepareLobby()
    {
        lobbyUI.SetActive(true);
        lobbyUIHandler = lobbyUI.GetComponent<LobbyUIHandler>();

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
        spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Game"));
    }

    internal void OnGameStepChange(string currentValue, string previousValue)
    {
        Debug.Log("New Gamestep:" + currentValue);

        switch (currentValue)
        {
            default:
                gameOverUI.SetActive(false);
                PrepareLobby();
                break;
            case "Starting":
                lobbyUI.SetActive(false);
                countdownUI.SetActive(true);
                StartCoroutine("CountDown");
                break;
            case "Ongoing":
                countdownUI.SetActive(false);
                break;
            case "Ended":
                gameOverUI.SetActive(true);
                break;
        }
    }

    public void ExitGame()
    {
        SceneManager.LoadScene("Main");
    }

    IEnumerator CountDown()
    {
        var countDownLabel = countdownUI.GetComponent<UIDocument>().rootVisualElement.Q<Label>("countdown-text");
        int i = 0;
        while (i < 5)
        {
            var countdownValue = 5 - i;
            countDownLabel.text = countdownValue.ToString();
            i++;
            yield return new WaitForSeconds(1);
        }

        countDownLabel.text = "Go!";
    }

    #endregion

    #region PlatformListeners

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

    #endregion

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
            //Debug.Log(playerSchema);
            playerController.playerSchema = playerSchema;
        });

        playerSchema.OnIsReadyChange((current, previous) => {
            //Debug.Log("ON IS READY CHANGE");
            //Debug.Log(current);
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
        if (lobbyUIHandler == null)
        {
            lobbyUIHandler = lobbyUI.GetComponent<LobbyUIHandler>();
        }
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
