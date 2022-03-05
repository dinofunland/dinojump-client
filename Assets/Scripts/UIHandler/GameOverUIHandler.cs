using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameOverUIHandler : MonoBehaviour
{
    // Start is called before the first frame update

    Button lobbyButton;
    Button exitButton;
    Label scoreText;
    Label rankText;

    void Start()
    {
        InitializeUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnExitGame_Clicked()
    {

    }

    void OnBackToLobby_Clicked()
    {

    }

    private void InitializeUI()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        lobbyButton = root.Q<Button>("lobby-button");
        exitButton = root.Q<Button>("exit-button");
        scoreText = root.Q<Label>("exit-button");
        lobbyButton.clicked += OnBackToLobby_Clicked;
        exitButton.clicked += OnExitGame_Clicked;
    }
}
