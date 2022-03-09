using Dinojump.Schemas;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float SpeedLerp = .02f;
    public PlayerSchema playerSchema;
    [SerializeField] public string playerKey;

    Animator playerAnimator;

    [SerializeField]
    AnimatorOverrideController blueAnim;
    [SerializeField]
    AnimatorOverrideController yellowAnim;
    [SerializeField]
    AnimatorOverrideController greenAnim;
    [SerializeField]
    AnimatorOverrideController purpleAnim;


    // Start is called before the first frame update
    void Start()
    {
        playerAnimator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        playerKey = playerSchema?.sessionId;

        if (playerSchema?.position != null)
        {
            var t = Time.deltaTime / SpeedLerp;
            Vector2 desiredPostion = new Vector3(playerSchema.position.x, playerSchema.position.y);
            transform.position = Vector2.Lerp(transform.position, desiredPostion, t);
        }

        return;
        SetPlayerSkin();

        
    }

    private void SetPlayerSkin()
    {
        //TODO: Change Player Sprite & Animator
        DinoPicker.DinoSkin current = (DinoPicker.DinoSkin)playerSchema.skin;
        switch (current)
        {
            default:
                playerAnimator.runtimeAnimatorController = blueAnim;
                break;
            case DinoPicker.DinoSkin.Yellow:
                playerAnimator.runtimeAnimatorController = yellowAnim;
                break;
            case DinoPicker.DinoSkin.Green:
                playerAnimator.runtimeAnimatorController = greenAnim;
                break;
            case DinoPicker.DinoSkin.Purple:
                playerAnimator.runtimeAnimatorController = purpleAnim;
                break;
        }
    }
}
