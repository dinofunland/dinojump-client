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
    private AnimationState currentAnimation;
    private bool currentLeft;
    private bool currentRight;


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
        if ((AnimationState)playerSchema?.animation != currentAnimation || playerSchema?.input.left != currentLeft || playerSchema?.input.right != currentRight)
        {
            SetAnimationState();
        }
    }

    private void SetAnimationState()
    {
        Debug.Log("AnimationState: " + playerSchema?.animation);
        Debug.Log("Left: " + playerSchema?.input.left + " Right: " + playerSchema?.input.right);
        currentAnimation = (AnimationState)playerSchema?.animation;
        if (playerSchema.input.left || playerSchema.input.right)
        {
            currentLeft = playerSchema.input.left;
            currentRight = playerSchema.input.right;
        }

        switch (currentAnimation)
        {
            default:
                SetIdleAnimation(currentLeft);
                break;
            case AnimationState.Walking:
                SetWalkAnimation(currentLeft);
                break;
            case AnimationState.Jumping:
                SetJumpAnimation(currentLeft);
                break;
            case AnimationState.Falling:
                SetFallingAnimation(currentLeft);
                break;
            case AnimationState.Dancing:
                SetDanceAnimation(currentLeft);
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

    private void SetWalkAnimation(bool toLeft)
    {
        playerAnimator.SetBool("isJumping", false);
        playerAnimator.SetBool("isWalking", true);
        playerAnimator.SetBool("isFalling", false);
        playerAnimator.SetBool("isDancing", false);
        gameObject.GetComponent<SpriteRenderer>().flipX = !toLeft;
    }
    private void SetJumpAnimation(bool toLeft)
    {
        playerAnimator.SetBool("isWalking", false);
        playerAnimator.SetBool("isJumping", true);
        playerAnimator.SetBool("isFalling", false);
        playerAnimator.SetBool("isDancing", false);
        gameObject.GetComponent<SpriteRenderer>().flipX = !toLeft;
    }
    private void SetIdleAnimation(bool toLeft)
    {
        playerAnimator.SetBool("isWalking", false);
        playerAnimator.SetBool("isJumping", false);
        playerAnimator.SetBool("isFalling", false);
        playerAnimator.SetBool("isDancing", false);
        gameObject.GetComponent<SpriteRenderer>().flipX = !toLeft;
    }
    private void SetFallingAnimation(bool toLeft)
    {
        return;
        playerAnimator.SetBool("isWalking", false);
        playerAnimator.SetBool("isJumping", false);
        playerAnimator.SetBool("isFalling", true);
        playerAnimator.SetBool("isDancing", false);
        gameObject.GetComponent<SpriteRenderer>().flipX = !toLeft;
    }
    private void SetDanceAnimation(bool toLeft)
    {
        return;
        playerAnimator.SetBool("isWalking", false);
        playerAnimator.SetBool("isJumping", false);
        playerAnimator.SetBool("isFalling", false);
        playerAnimator.SetBool("isDancing", true);
        gameObject.GetComponent<SpriteRenderer>().flipX = !toLeft;
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
