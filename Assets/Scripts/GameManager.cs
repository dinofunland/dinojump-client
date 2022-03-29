using Colyseus.Schema;
using Dinojump;
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
    ScoreUIHandler scoreUIHandler;
  
    SpawnManager spawnManager;

    [SerializeField]
    public GameObject playerPrefab;
    [SerializeField]
    private GameObject scoreUI;
    [SerializeField]
    private GameObject countdownUI;
    [SerializeField]
    private GameObject gameOverUI;
    [SerializeField]
    private GameObject lobbyUI;

    public Dictionary<string, PlayerSchema> playerList = new Dictionary<string, PlayerSchema>();

    public int Score;
    public string myPlayerKey;

    public List<string> ErrorMessages { get; private set; }

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
        if (SceneManager.sceneCount == 1)
        {
            SceneManager.LoadScene("Menu", LoadSceneMode.Additive);
        }
    }

    
    public void SaveVolumeValue(float volume)
    {
        PlayerPrefs.SetFloat("volume", volume);
    }

    public float LoadVolumeValue()
    {
        float retVal = 0.5f;
        if (PlayerPrefs.HasKey("playername"))
        {
            retVal =  PlayerPrefs.GetFloat("volume");
        }
        return retVal;
    }

    void ConstructGroundAndWalls()
    {
        //TODO: Change WallSize on Server
        float offset = 5f;

        var groundSchema = RoomManager.Instance.colyseusRoom.State.ground;

        var wallLeft = GameObject.Find("WallLeft");
        var wallRight = GameObject.Find("WallRight");
        var ground = GameObject.Find("Ground");
        var wallWidth = wallLeft.GetComponentInChildren<SpriteRenderer>().bounds.extents.x;
        var groundHeight = ground.GetComponentInChildren<SpriteRenderer>().bounds.extents.y + offset;

        var halfGroundWidth = groundSchema.size.width / 2;

        wallLeft.transform.position = new Vector2(-halfGroundWidth - wallWidth, wallLeft.transform.position.y);
        wallRight.transform.position = new Vector2(halfGroundWidth + wallWidth, wallRight.transform.position.y);

        ground.transform.position = new Vector2(groundSchema.position.x, groundSchema.position.y - groundHeight);
    }

    internal void OnLeaveLobby(int code)
    {
        LoadMain("Connection closed: " + code);
    }

    internal void OnLobbyError(int code, string message)
    {
        LoadMain("Lobby Error " + code.ToString() + ": " + message);
    }

    internal void OnScoreChange(float currentValue, float previousValue)
    {
        Score = (int)Mathf.Ceil(currentValue);
        if (Score > 0)
        { 
            if(scoreUIHandler == null)
                scoreUIHandler = scoreUI.GetComponent<ScoreUIHandler>();
            scoreUIHandler.SetNewScore(Score);
        }
    }

    internal void OnFloorPositionChange(FloorSchema currentValue, FloorSchema previousValue)
    {
        Debug.Log("Floor Schema changed.");
        var lavaObject = GameObject.Find("LavaContainer").GetComponent<LavaController>();
        lavaObject.floorSchema = currentValue;
    }

    #region LobbyHandling

    //Join Lobby
    public async void ConnectToLobby(string playerName, string code = null)
    {
        try
        {
            LoadLobby();
            if(code != null)
                await RoomManager.Instance.ConnectLobby(playerName, code);
            else
                await RoomManager.Instance.ConnectLobby(playerName);

            AudioManager.Instance.SetGameTheme();
        }
        catch (Exception ex)
        {
            LoadMain(ex.Message);
        }
    }

    private void LoadLobby()
    {
        Instance.ErrorMessages = new List<string>();
        Instance.playerList = new Dictionary<string, PlayerSchema>();
        Instance.myPlayerKey = null;
        SceneManager.UnloadSceneAsync("Menu");
        StartCoroutine(AwaitGameScene());
    }
    public void LoadMain(string error = null)
    {
        if (!string.IsNullOrEmpty(error))
        {
            Instance.ErrorMessages.Add(error);
        }
        
        lobbyUI.SetActive(false);
        gameOverUI.SetActive(false);
        scoreUI.SetActive(false);
        countdownUI.SetActive(false);

        SceneManager.UnloadSceneAsync("Game");
        SceneManager.LoadSceneAsync("Menu", LoadSceneMode.Additive);
    }

    void PrepareLobby()
    {       
        countdownUI.SetActive(false);
        lobbyUI.SetActive(true);
        lobbyUIHandler = lobbyUI.GetComponent<LobbyUIHandler>();

        if (RoomManager.Instance.colyseusRoom != null)
        {
            lobbyUIHandler.SetLobbyCode(RoomManager.Instance.colyseusRoom.RoomId);
        }

        ConstructGroundAndWalls();
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
                scoreUI.SetActive(true);
                break;
            case "Ongoing":
                countdownUI.SetActive(false);
                break;
            case "Ended":
                scoreUI.SetActive(false);
                //foreach (PlatformBase pb in FindObjectsOfType<PlatformBase>())
                //    Destroy(pb.gameObject);
                gameOverUI.SetActive(true);
                break;
        }
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

    public void OnEmoteMessage(EmoteMessage emoteMessage)
    {
        var player = FindObjectsOfType<PlayerController>().FirstOrDefault(m => m.playerKey == emoteMessage.sessionId);
        if (player != null)
        {
            player.HandleEmote(emoteMessage.emoteType);
        }
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
            OnPlayerChange(playerSchema.sessionId, playerSchema);
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
