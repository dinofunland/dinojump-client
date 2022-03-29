using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField]
    float horizontalInput;
    bool jumpInput;
    bool modulationInput;

    bool lastEmoteButtonInput;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        var newHorizontalInput = Input.GetAxisRaw("Horizontal");
        var hasHorizontalInputChanged = newHorizontalInput != horizontalInput;
        if (hasHorizontalInputChanged)
        {
            horizontalInput = newHorizontalInput;
            SendPlayerMovement();
        }

        var newJumpInput = Input.GetButton("Jump");
        var hasJumpInputChanged = newJumpInput != jumpInput;
        if (hasJumpInputChanged)
        {
            jumpInput = newJumpInput;
            if(jumpInput) {
                SendPlayerJump();
            }
        }

        var readyInput = Input.GetButton("Ready");
        if (readyInput)
        {
            var lobbyUI = FindObjectOfType<LobbyUIHandler>();
            if (lobbyUI != null)
                lobbyUI.OnReady_Clicked();
        }

            // TODO handle modulationInput
            modulationInput = Input.GetButton("Fire3");

        var player = FindObjectsOfType<PlayerController>().FirstOrDefault(m => m.playerKey == GameManager.Instance.myPlayerKey);
        EmoteWheel emoteWheel = null;
        
        if(player != null)
            emoteWheel = player.gameObject.GetComponentInChildren<EmoteWheel>();

        if (emoteWheel != null && !emoteWheel.isWheelOpen)
        {
            var emoteButton = Input.GetButton("Emote");
            if (lastEmoteButtonInput != emoteButton)
            {
                lastEmoteButtonInput = emoteButton;
                if(emoteButton)
                 emoteWheel.OpenWheel();
            }
        }
    }

    async void SendCryEmote()
    {
        await RoomManager.Instance?.colyseusRoom.Send("emote", (int)EmoteController.EmoteType.Cry);
    }
    async void SendHeartEmote()
    {
        await RoomManager.Instance?.colyseusRoom.Send("emote", (int)EmoteController.EmoteType.Heart);
    }

    async void SendLaughEmote()
    {
        await RoomManager.Instance?.colyseusRoom.Send("emote", (int)EmoteController.EmoteType.Laugh);
    }

    async void SendThumbsUpEmote()
    {
        await RoomManager.Instance?.colyseusRoom.Send("emote", (int)EmoteController.EmoteType.ThumbsUp);
    }


    async void SendPlayerMovement()
    {
        if (RoomManager.Instance?.colyseusRoom != null)
        {
            await RoomManager.Instance.colyseusRoom.Send("inputHorizontal", horizontalInput);
        }
    }

    async void SendPlayerJump()
    {
        if (RoomManager.Instance?.colyseusRoom != null)
        {
            await RoomManager.Instance.colyseusRoom.Send("jump");
        }
    }
}
