using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LobbyUIHandler : MonoBehaviour
{
    
    Label lobbyCode;
    VisualElement playerContainer;
    Button readyButton;


    // Start is called before the first frame update
    void Start()
    {
        InitializeUI();
    }



    // Update is called once per frame
    void Update()
    {
        
    }

    void OnReady_Clicked()
    {
        Debug.Log("Player ready clicked.");
    }
    private void InitializeUI()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        lobbyCode = root.Q<Label>("lobby-code-text");
        playerContainer = root.Q<VisualElement>("player-container");
        readyButton = root.Q<Button>("ready-button");
        readyButton.clicked += OnReady_Clicked;


        //TODO: replace with actual Players
        playerContainer.Clear();
        for (int i = 1; i < 4; i++)
        {
            var label = new Label("#Player "+ i);
            label.AddToClassList("player-list-item");

            playerContainer.Add(label);
        }
        
    }
}
