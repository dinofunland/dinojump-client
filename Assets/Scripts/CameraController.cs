using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CameraController : MonoBehaviour
{
    public Vector3 OffsetCamera;
    public float SpeedCamera;
    public bool IsFocusingPlayer = true;
    public Transform CameraTarget;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (IsFocusingPlayer)
        {
            var playerControllers = GameObject.FindObjectsOfType<PlayerController>();
            var playerController = playerControllers.FirstOrDefault(playerController => playerController.playerKey == GameManager.Instance.myPlayerKey);
            if (playerController)
            {
                CameraTarget = playerController.gameObject.transform;
            }
        }
        if(CameraTarget == null)
        {
            transform.position = new Vector3(0, 0, -30);
        }
    }

    void LateUpdate()
    {
        if (IsFocusingPlayer)
        {
            if (CameraTarget != null)
            {
                var desiredPosition = CameraTarget.position + OffsetCamera;
                desiredPosition.x = 0;
                var t = Time.deltaTime * SpeedCamera;
                transform.position = Vector3.Lerp(transform.position, desiredPosition, t);
            }

        }
    }
}
