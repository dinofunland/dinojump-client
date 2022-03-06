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
        horizontalInput = Input.GetAxis("Horizontal");
        jumpInput = Input.GetButton("Jump");
        modulationInput = Input.GetButton("Fire3");
        if (RoomManager.Instance?.colyseusRoom != null)
        {
            SendPLayerMovement();
        }
    }

    async void SendPLayerMovement()
    {
        if (horizontalInput == 0)
            await RoomManager.Instance.colyseusRoom.Send("move", new { left = false, right = false });
        if (horizontalInput > 0)
            await RoomManager.Instance.colyseusRoom.Send("move", new { left = false, right = true });
        if (horizontalInput < 0)
            await RoomManager.Instance.colyseusRoom.Send("move", new { left = true, right = false });
        if (jumpInput)
            await RoomManager.Instance.colyseusRoom.Send("jump");
        if (modulationInput)
            await RoomManager.Instance.colyseusRoom.Send("mod");
    }
}