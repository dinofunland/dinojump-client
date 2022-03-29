using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class StartMenuUIHandler : MonoBehaviour
{
    TextField lobbyCodeText;
    TextField playerNameText;
    Button newLobbyButton;
    Button joinLobbyButton;
    VisualElement errorMessagesContainer;
    [SerializeField] AudioSource audioManager;

    // Start is called before the first frame update
    void OnEnable()
    {
        InitializeUI();
    }    

    // Update is called once per frame
    void Update()
    {
        
    }

    private void InitializeUI()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        lobbyCodeText = root.Q<TextField>("lobby-code-input");
        playerNameText = root.Q<TextField>("player-name-input");
        playerNameText.SetValueWithoutNotify(LoadPlayerName());
        newLobbyButton = root.Q<Button>("new-lobby-button");
        joinLobbyButton = root.Q<Button>("join-lobby-button");

        var volumeSlider = root.Q<Slider>("volumeslider");
        var savedVolume = GameManager.Instance.LoadVolumeValue();
        volumeSlider.SetValueWithoutNotify(savedVolume);
        audioManager.volume = savedVolume;
        AudioManager.Instance.SetMenuTheme();
        volumeSlider.RegisterValueChangedCallback(v =>
        {
            var oldValue = v.previousValue;
            var newValue = v.newValue;
            GameManager.Instance.SaveVolumeValue(newValue / 100);
            AudioManager.Instance.SetVolume(v.newValue / 100);
        });

        newLobbyButton.clicked += OnNewLobby_Clicked;
        joinLobbyButton.clicked += OnJoinLobby_Clicked;

        errorMessagesContainer = root.Q<VisualElement>("error-messages");
        errorMessagesContainer.Clear();

        if (GameManager.Instance != null && GameManager.Instance.ErrorMessages != null && GameManager.Instance.ErrorMessages.Any())
        {
            foreach (var err in GameManager.Instance.ErrorMessages)
            {
                Label l = new Label();
                l.text = err;
                errorMessagesContainer.Add(l);
            }
        }
    }

    void OnNewLobby_Clicked()
    {
        if (!string.IsNullOrEmpty(playerNameText.text))
        {
            SavePlayerName(playerNameText.text);
            GameManager.Instance.ConnectToLobby(playerNameText.text);
        } 
    }


    void OnJoinLobby_Clicked()
    {
        if (!string.IsNullOrEmpty(lobbyCodeText.text)  && !string.IsNullOrEmpty(playerNameText.text))
        {
            SavePlayerName(playerNameText.text);
            GameManager.Instance?.ConnectToLobby(playerNameText.text, lobbyCodeText.text);
        }
    }

    void SavePlayerName(string name)
    {
        PlayerPrefs.SetString("playername", name);
    }
    string LoadPlayerName()
    {
        string retVal = "";
        if (PlayerPrefs.HasKey("playername"))
        {
            retVal = PlayerPrefs.GetString("playername");
        }
        return retVal;
    }

}
