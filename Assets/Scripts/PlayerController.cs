using Dinojump.Schemas;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{    
    [SerializeField]
    private float SpeedLerp = 10000f;
    float splashOffset = 8f;
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

    SpriteRenderer spriteRenderer;

    private DinoPicker.DinoSkin currentSkin;
    private AnimationState currentAnimation;
    private AnimationState previousAnimation;
    private bool currentLeft;
    private bool currentRight;

    private bool deathAnimationSet;

    bool isWalking;
    bool isJumping;
    bool isFalling; 
    bool isDancing;
    bool isDead;

    // Start is called before the first frame update
    void Start()
    {
        currentSkin = DinoPicker.DinoSkin.Blue;
        audioPlayer = GetComponent<AudioSource>();
        playerAnimator = GetComponent<Animator>();
        jumpSound.LoadAudioData();
        dropSound.LoadAudioData();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        playerKey = playerSchema?.sessionId;
        
        if (playerSchema?.position != null && !deathAnimationSet)
        {
            var t = Time.deltaTime * SpeedLerp;
            Vector2 desiredPostion = new Vector3(playerSchema.position.x, playerSchema.position.y);
            transform.position = Vector2.Lerp(transform.position, desiredPostion, t);
        }
        if (!playerSchema.isDead && deathAnimationSet)
        {
            Debug.Log("Player alive again.");
            spriteRenderer.sortingOrder = 0;
            deathAnimationSet = false;
            playerAnimator.SetBool("isDead", false);
        }
    }

    private void LateUpdate()
    {
        //Debug.Log("Server AnimationState: " + playerSchema.animation);
        if (playerSchema?.skin != (int)currentSkin)
        {
            SetPlayerSkin();
        }
        if (playerSchema.isDead && !deathAnimationSet)
        {
            spriteRenderer.sortingOrder = 999;
            Vector3 newPos = transform.position;

            //Set LavaSplash on Top of LavaLake
            var floor = FindObjectOfType<LavaController>();
            newPos.y = floor.GetComponentInChildren<SpriteRenderer>().bounds.extents.y + floor.transform.position.y + splashOffset;
            transform.position = newPos;

            audioPlayer.PlayOneShot(dropSound);
            SetDeathAnimation();
        }
        if ((AnimationState)playerSchema?.animation != currentAnimation)
        {
            if (!deathAnimationSet)
            {
                SetAnimationState();
            } 
        }
        if (playerSchema?.input.left != currentLeft || playerSchema?.input.right != currentRight)
        {
            if (playerSchema.input.left || playerSchema.input.right)
            {
                currentLeft = playerSchema.input.left;
                currentRight = playerSchema.input.right;
            }
            gameObject.GetComponent<SpriteRenderer>().flipX = !currentLeft;
        }
    }

    private void SetAnimationState()
    {
        previousAnimation = this.currentAnimation;
        currentAnimation = (AnimationState)playerSchema?.animation;

        switch (currentAnimation)
        {
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
            default:
                    SetIdleAnimation();
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
        isWalking = false;
        isJumping = false;
        isFalling = false;
        isDancing = false;
        isDead = false;

        SetAnimationVariables();
    }

    private void SetAnimationVariables()
    {
        playerAnimator.SetBool("isWalking", isWalking);
        playerAnimator.SetBool("isJumping", isJumping);
        playerAnimator.SetBool("isFalling", isFalling);
        playerAnimator.SetBool("isDancing", isDancing);
        playerAnimator.SetBool("isDead", isDead);
    }

    private void SetWalkAnimation()
    {
        playerAnimator.SetBool("isWalking", true);
        playerAnimator.SetBool("isJumping", false);
    }
    private void SetJumpAnimation()
    {
        playerAnimator.SetBool("isJumping", true);
        playerAnimator.SetTrigger("jump");
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
        Debug.Log("DeathAnimationSet");
        ResetAnimationVariables();
        playerAnimator.SetBool("isDead", true);
        deathAnimationSet = true;
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
