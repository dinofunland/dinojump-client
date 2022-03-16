using Dinojump.Schemas;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{    
    [SerializeField]
    private float SpeedLerp = .02f;
    public PlayerSchema playerSchema;
    [SerializeField] public string playerKey;

    Animator playerAnimator;

    [SerializeField] AnimatorOverrideController blueAnim;
    [SerializeField] AnimatorOverrideController yellowAnim;
    [SerializeField] AnimatorOverrideController greenAnim;
    [SerializeField] AnimatorOverrideController purpleAnim;

    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip dropSound;
    private AudioSource audioPlayer;

    private DinoPicker.DinoSkin currentSkin;
    private AnimationState currentAnimation;
    private AnimationState previousAnimation;
    private bool currentLeft;
    private bool currentRight;


    // Start is called before the first frame update
    void Start()
    {
        currentSkin = DinoPicker.DinoSkin.Blue;
        audioPlayer = GetComponent<AudioSource>();
        playerAnimator = GetComponent<Animator>();
        jumpSound.LoadAudioData();
        dropSound.LoadAudioData();
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
        if ((AnimationState)playerSchema?.animation != currentAnimation || playerSchema?.input.left != currentLeft || playerSchema?.input.right != currentRight)
        {
            SetAnimationState();
        }
    }

    private void SetAnimationState()
    {
        previousAnimation = this.currentAnimation;
        currentAnimation = (AnimationState)playerSchema?.animation;
        if (playerSchema.input.left || playerSchema.input.right)
        {
            currentLeft = playerSchema.input.left;
            currentRight = playerSchema.input.right;
        }


        ResetAnimationVariables();

        if (false)//playerSchema.isDead)
        {
            //TODO Trigger Death Animation
            return;

            audioPlayer.PlayOneShot(dropSound);
            SetDeathAnimation();
            return;
        }
        gameObject.GetComponent<SpriteRenderer>().flipX = !currentLeft;
        switch (currentAnimation)
        {
            default:
                SetIdleAnimation();
                break;
            case AnimationState.Walking:
                SetWalkAnimation();
                break;
            case AnimationState.Jumping:
                if (previousAnimation != AnimationState.Jumping && previousAnimation != AnimationState.Falling)
                {
                    audioPlayer.PlayOneShot(jumpSound);
                    SetJumpAnimation();
                }
                break;
            case AnimationState.Falling:
                SetFallingAnimation();
                break;
            case AnimationState.Dancing:
                SetDanceAnimation();
                break;
        }
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

    private void ResetAnimationVariables()
    {
        playerAnimator.SetBool("isWalking", false);
        playerAnimator.SetBool("isJumping", false);
        playerAnimator.SetBool("isFalling", false);
        playerAnimator.SetBool("isDancing", false);
        playerAnimator.SetBool("isDead", false);
    }

    private void SetWalkAnimation()
    {
        playerAnimator.SetBool("isWalking", true); 
    }
    private void SetJumpAnimation()
    {
        playerAnimator.SetBool("isJumping", true);
    }
    private void SetIdleAnimation()
    {
        ResetAnimationVariables();
    }
    private void SetFallingAnimation()
    {
        //TODO: Create Sprites, Animation and add to AnimatorController
        return;
        playerAnimator.SetBool("isFalling", true);

    }
    private void SetDanceAnimation()
    {
        //TODO: Create Sprites, Animation and add to AnimatorController
        return;
        playerAnimator.SetBool("isDancing", true);

    }

    private void SetDeathAnimation()
    {
        return;
        playerAnimator.SetBool("isDead", true);
    }

    enum AnimationState
    { 
        Idle = 0,
        Walking = 1,
        Jumping = 2,
        Falling = 3,
        Dancing = 4
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Vector3 size = new Vector3(playerSchema.size.width, playerSchema.size.height);
        Gizmos.DrawWireCube(this.gameObject.transform.position, size);
    }
}
