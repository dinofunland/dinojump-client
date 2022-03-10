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
        var newHorizontalInput = Input.GetAxis("Horizontal");
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

        // TODO handle modulationInput
        modulationInput = Input.GetButton("Fire3");
    }

    async void SendPlayerMovement()
    {
        if (RoomManager.Instance?.colyseusRoom != null)
        {
            if (horizontalInput == 0)
                await RoomManager.Instance.colyseusRoom.Send("move", new { left = false, right = false });
            if (horizontalInput > 0)
                await RoomManager.Instance.colyseusRoom.Send("move", new { left = false, right = true });
            if (horizontalInput < 0)
                await RoomManager.Instance.colyseusRoom.Send("move", new { left = true, right = false });
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
