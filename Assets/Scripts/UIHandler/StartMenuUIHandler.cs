using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class StartMenuUIHandler : MonoBehaviour
{
    TextField lobbyCodeText;
    TextField playerNameText;
    Button newLobbyButton;
    Button joinLobbyButton;

    // Start is called before the first frame update
    void Start()
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
        newLobbyButton = root.Q<Button>("new-lobby-button");
        joinLobbyButton = root.Q<Button>("join-lobby-button");

        newLobbyButton.clicked += OnNewLobby_Clicked;
        joinLobbyButton.clicked += OnJoinLobby_Clicked;
    }

    void OnNewLobby_Clicked()
    {
        if (!string.IsNullOrEmpty(playerNameText.text))
        {
            //GameManager.Instance.PlayerName = playerNameText.text;
            GameManager.Instance.EnterLobby(playerNameText.text);
        } 
    }

    void OnJoinLobby_Clicked()
    {
        if (!string.IsNullOrEmpty(lobbyCodeText.text)  && !string.IsNullOrEmpty(playerNameText.text))
        {
            GameManager.Instance.EnterLobby(playerNameText.text,lobbyCodeText.text);
        }
    }
}
