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

    private DinoPicker.DinoSkin currentSkin;


    // Start is called before the first frame update
    void Start()
    {
        currentSkin = DinoPicker.DinoSkin.Blue;
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
        if (playerSchema?.skin != (int)currentSkin)
        {
            SetPlayerSkin();
        }

        return;
        SetJumpAnimation();
        //TODO: implement on server / in playerschema
        //switch (playerSchema.AnimationState)
        //{
        //    default:
        //        SetIdleAnimation();
        //        break;
        //    case AnimationState.Walking:
        //        SetWalkAnimation(playerSchema.facingLeft);
        //        break;
        //    case AnimationState.Jumping:
        //        SetJumpAnimation();
        //        break;
        //}
    }

    private void SetPlayerSkin()
    {
        DinoPicker.DinoSkin newSkin = (DinoPicker.DinoSkin)playerSchema?.skin;
        currentSkin = newSkin;
        switch (newSkin)
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

    private void SetWalkAnimation(bool toLeft)
    {
        playerAnimator.SetBool("isJumping", false);
        playerAnimator.SetBool("isWalking", true);
        gameObject.GetComponent<SpriteRenderer>().flipY = toLeft;
    }
    private void SetJumpAnimation()
    {
        playerAnimator.SetBool("isWalking", false);
        playerAnimator.SetBool("isJumping", true);
    }
    private void SetIdleAnimation()
    {
        playerAnimator.SetBool("isWalking", false);
        playerAnimator.SetBool("isJumping", false);
    }

    enum AnimationState
    { 
        Idle = 1,
        Walking = 2,
        Jumping = 3
    }
}
