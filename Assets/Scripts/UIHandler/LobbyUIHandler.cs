using Dinojump.Schemas;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class LobbyUIHandler : MonoBehaviour
{
    Label lobbyCode;
    VisualElement playerContainer;
    Button readyButton;
    Button leaveButton;

    Button btnBlue;
    Button btnGreen;
    Button btnPurp;
    Button btnYellow;


    // Start is called before the first frame update
    void OnEnable()
    {
        InitializeUI();
    }

    public async void OnReady_Clicked()
    {
        if(RoomManager.Instance?.colyseusRoom != null)
        {
            try
            {
                await RoomManager.Instance.colyseusRoom.Send("ready");
                readyButton.AddToClassList("is-ready-button");
                readyButton.text = "Ready!";
                readyButton.SetEnabled(false);
                //RenderPlayerNames();
                //Destroy(gameObject);
            }
            catch 
            {
                readyButton.RemoveFromClassList("is-ready-button");
                readyButton.text = "Ready?";
                readyButton.SetEnabled(true);
            }     
        }
    }

    async void OnLeave_Clicked()
    {
        if (RoomManager.Instance?.colyseusRoom != null)
        {
            try
            {
                await RoomManager.Instance.colyseusRoom.Leave();
            }
            catch (Exception ex)
            {
                GameManager.Instance.LoadMain(ex.Message);
                Debug.Log("Exception while Leaving Room: " + ex);
            }
        }
        GameManager.Instance.LoadMain();
    }

    private void InitializeUI()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        lobbyCode = root.Q<Label>("lobby-code-text");
        playerContainer = root.Q<VisualElement>("player-container");
        readyButton = root.Q<Button>("ready-button");
        readyButton.clicked += OnReady_Clicked;
        readyButton.SetEnabled(true);
        playerContainer.Clear();

        leaveButton = root.Q<Button>("leave-button");
        leaveButton.clicked += OnLeave_Clicked;
        leaveButton.SetEnabled(true);
    }

    public void SetLobbyCode(string code)
    {
        if (lobbyCode == null)
            InitializeUI();
        lobbyCode.text = code;
    }
    public void RenderPlayerNames()
    {
        //clear and redraw list
        playerContainer.Clear();
        
        var newList = new Dictionary<string, PlayerSchema>();
        foreach (var player in GameManager.Instance.playerList)
        {
            if (player.Value != null)
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
                newList.Add(player.Key, player.Value);
            }
        }
        GameManager.Instance.playerList = newList;
    }
}
