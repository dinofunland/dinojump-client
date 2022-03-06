using Dinojump.Schemas;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LobbyUIHandler : MonoBehaviour
{

    Dictionary<string, PlayerSchema> playerDict;

    Label lobbyCode;
    VisualElement playerContainer;
    Button readyButton;


    // Start is called before the first frame update
    void Start()
    {
        playerDict = new Dictionary<string, PlayerSchema>();
        InitializeUI();
    }



    // Update is called once per frame
    void Update()
    {

    }

    async void OnReady_Clicked()
    {
        await RoomManager.Instance.colyseusRoom.Send("ready");
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
