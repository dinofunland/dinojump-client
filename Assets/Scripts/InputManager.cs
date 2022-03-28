using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField]
    float horizontalInput;
    bool jumpInput;
    bool modulationInput;
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

        if(Input.GetButton("Emote"))
        {
            SendCryEmote();
        }
    }

    async void SendCryEmote()
    {
        await RoomManager.Instance?.colyseusRoom.Send("emote", (int)EmoteController.EmoteType.Cry);
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
