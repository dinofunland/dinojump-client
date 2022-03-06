using Dinojump.Schemas;
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

    async void OnReady_Clicked()
    {
        if(RoomManager.Instance?.colyseusRoom != null)
        {
            await RoomManager.Instance.colyseusRoom.Send("ready");
        }
    }

    private void InitializeUI()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        lobbyCode = root.Q<Label>("lobby-code-text");
        playerContainer = root.Q<VisualElement>("player-container");
        readyButton = root.Q<Button>("ready-button");
        readyButton.clicked += OnReady_Clicked;
        playerContainer.Clear();
    }

    public void SetLobbyCode(string code)
    {
        lobbyCode.text = code;
    }
    public void RenderPlayerNames()
    {
        //clear and redraw list
        playerContainer.Clear();
        foreach (var player in GameManager.Instance.playerList)
        {
            var label = new Label(player.Value.username);
            if (player.Value.isReady)
            {
                label.AddToClassList("player-list-item-ready");
            }
            else
            {
                label.AddToClassList("player-list-item");
            }

            playerContainer.Add(label);
        }
    }
}