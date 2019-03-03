using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterController : CharacterMovement
{
    protected Animator animator;
    
    public bool isJabbing;
    public bool canJab;

    public bool isTumbling;
    public bool isTumbled;

    public bool isShielding;
    public bool isRollingForward;
    public bool isRollingBackward;
    public float rollSpeed;
    
    public bool isGrabbing;
    public bool isGrabbed;
    public GameObject grabbingFighter;

    //Action Buffer Queue Veriables
    public float actionBufferWindow;
    protected float actionBufferTimeLeft;
    protected ActionInput actionInputQueue;
    
    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    protected override void InitializeComponents() {
        base.InitializeComponents();
        animator = GetComponent<Animator>();
    }

    protected override void InitializeVariables() {
        base.InitializeVariables();
        actionBufferTimeLeft = actionBufferWindow;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateInput();
        UpdateAnimation();
    }

    void FixedUpdate() {
        Tick();
    }

    protected override void Tick() {
        if (isRollingForward) {
            overridingVelocity = new Vector2(isFacingRight ? rollSpeed : -rollSpeed, 0);
        }
        if (isRollingBackward) {
            overridingVelocity = new Vector2(isFacingRight ? -rollSpeed : rollSpeed, 0);
        }
        base.Tick();
    }

    protected override void UpdateInput() {
        base.UpdateInput();
        //Update Action Buffer
        if (actionInputQueue != null) {
            //Special case for jab
            if (isJabbing) {
                if (actionInputQueue.inputType == InputType.Attack) {
                    if (actionInputQueue.inputStrength == InputStrength.None) {
                        if (canJab) {
                            animator.SetTrigger("Jab");
                            ClearActionBuffer();
                        }
                    }
                }
            }
            if (!isBusy) {
                ProcessActionInput();
                ClearActionBuffer();
                return;
            }
            actionBufferTimeLeft -= Time.deltaTime;
            if (actionBufferTimeLeft < 0) {
                ClearActionBuffer();
                return;
            }
        }
    }

    protected virtual void UpdateAnimation() {
        animator.SetBool("isBusy", isBusy);
        animator.SetBool("isGrounded", isGrounded);
        animator.SetBool("isJabbing", isJabbing);
        animator.SetBool("isShielding", isShielding);
        if (!isBusy) {
            animator.SetBool("isJumpSquatting", isJumpSquatting);
            if (isGrounded) {
                animator.SetBool("isWalking", isWalking);
                animator.SetBool("isDashing", isDashing);
                animator.SetBool("isCrouching", isCrouching);
                animator.SetFloat("WalkSpeed", velocity.x);
            }
            else {
                
            }
            
        }
    }

    protected virtual void ProcessActionInput() {
        if (actionInputQueue.inputType == InputType.Special) {
            ProcessSpecialInput();
            return;
        }
        //Grounded Actions
        if (isGrounded) {
            //Ground Attack Actions
            switch (actionInputQueue.inputType) {
            case InputType.Attack:
                //Dash Attack
                if (isDashing) {
                    animator.SetTrigger("DashAttack");
                    return;
                }
                //Grab Pummel
                if (isGrabbing) {
                    animator.SetTrigger("GrabPummel");
                    return;
                }
                switch (actionInputQueue.inputStrength) {
                //Jab
                case InputStrength.None:
                    animator.SetTrigger("Jab");
                    return;
                //Tilts
                case InputStrength.Weak:
                    switch (actionInputQueue.inputDirection) {
                    case InputDirection.Right:
                        isFacingRight = true;
                        FlipCheck();
                        animator.SetTrigger("ForwardTilt");
                        return;
                    case InputDirection.Left:
                        isFacingRight = false;
                        FlipCheck();
                        animator.SetTrigger("ForwardTilt");
                        return;
                    case InputDirection.Up:
                        animator.SetTrigger("UpTilt");
                        return;
                    case InputDirection.Down:
                        animator.SetTrigger("DownTilt");
                        return;
                    }
                    break;
                //Smashes
                case InputStrength.Strong:
                    switch (actionInputQueue.inputDirection) {
                    case InputDirection.Right:
                        isFacingRight = true;
                        FlipCheck();
                        animator.SetTrigger("ForwardSmash");
                        return;
                    case InputDirection.Left:
                        isFacingRight = false;
                        FlipCheck();
                        animator.SetTrigger("ForwardSmash");
                        return;
                    case InputDirection.Up:
                        animator.SetTrigger("UpSmash");
                        return;
                    case InputDirection.Down:
                        animator.SetTrigger("DownSmash");
                        return;
                    }
                    break;
                }
                break;
            //Ground Special Actions
            case InputType.Special:
                ProcessSpecialInput();
                break;
            //Ground Shield Actions
            case InputType.Shield:
                switch (actionInputQueue.inputStrength) {
                //Shield
                case InputStrength.None:
                    isShielding = true;
                    return;
                //Dodge Movements
                default:
                    switch (actionInputQueue.inputDirection) {
                    case InputDirection.Right:
                        if (isFacingRight) animator.SetTrigger("RollForward");
                        else animator.SetTrigger("RollBackward");
                        return;
                    case InputDirection.Left:
                        if (isFacingRight) animator.SetTrigger("RollBackward");
                        else animator.SetTrigger("RollForward");
                        return;
                    case InputDirection.Up:
                        Jump();
                        return;
                    case InputDirection.Down:
                        animator.SetTrigger("SpotDodge");
                        return;
                    }
                    break;
                }
                break;
            //Ground Grab Actions
            case InputType.Grab:
                switch (actionInputQueue.inputStrength) {
                //Grab
                case InputStrength.None:
                    return;
                //Directional Grab
                default:
                    switch (actionInputQueue.inputDirection) {
                    case InputDirection.Right:
                        break;
                    case InputDirection.Left:
                        break;
                    default:
                        break;
                    }
                    break;
                }
                break;
            }
        }
        //Aerial Actions
        else {
            //Aerial Attack Actions
            switch (actionInputQueue.inputType) {
            case InputType.Attack:
                switch (actionInputQueue.inputStrength) {
                //Neutral Air
                case InputStrength.None:
                    animator.SetTrigger("NeutralAir");
                    return;
                //Directional Air Attacks
                default:
                    switch (actionInputQueue.inputDirection) {
                    case InputDirection.Right:
                        if (isFacingRight) {
                            animator.SetTrigger("ForwardAir");
                            return;
                        }
                        animator.SetTrigger("BackAir");
                        return;
                    case InputDirection.Left:
                        if (isFacingRight) {
                            animator.SetTrigger("BackAir");
                            return;
                        }
                        animator.SetTrigger("ForwardAir");
                        return;
                    case InputDirection.Up:
                        animator.SetTrigger("UpAir");
                        return;
                    case InputDirection.Down:
                        animator.SetTrigger("DownAir");
                        return;
                    }
                    break;
                }
                break;
            //Aerial Special Actions
            case InputType.Special:
                ProcessSpecialInput();
                break;
            //Aerial Shield Actions
            case InputType.Shield:
                switch (actionInputQueue.inputStrength) {
                //Air Dodge
                case InputStrength.None:
                    break;
                //Directional Air Dodge
                default:
                    switch (actionInputQueue.inputDirection) {
                    case InputDirection.Right:
                        break;
                    case InputDirection.Left:
                        break;
                    case InputDirection.Up:
                        break;
                    case InputDirection.Down:
                        break;
                    }
                    break;
                }
                break;
            //Aerial Grab Actions
            case InputType.Grab:
                //Zair
                break;
            }
        }
    }

    protected virtual void ProcessSpecialInput() {
        switch (actionInputQueue.inputStrength) {
        //Neutral Special
        case InputStrength.None:
            animator.SetTrigger("NeutralSpecial");
            return;
        //Directional Specials
        default:
            switch (actionInputQueue.inputDirection) {
            case InputDirection.Right:
                isFacingRight = true;
                FlipCheck();
                animator.SetTrigger("SideSpecial");
                return;
            case InputDirection.Left:
                isFacingRight = false;
                FlipCheck();
                animator.SetTrigger("SideSpecial");
                return;
            case InputDirection.Up:
                animator.SetTrigger("UpSpecial");
                return;
            case InputDirection.Down:
                animator.SetTrigger("DownSpecial");
                return;
            }
            break;
        }
    }

    protected virtual void ClearActionBuffer() {
        actionBufferTimeLeft = actionBufferWindow;
        actionInputQueue = null;
    }

    public void ForwardRollStart() {
        OverrideVelocity(Vector2.zero);
    }

    public void ForwardRollMovement() {
        isRollingForward = true;
        Vector2 direction = new Vector2(isFacingRight ? rollSpeed : -rollSpeed, 0);
        OverrideVelocity(direction);
    }

    public void ForwardRollEnd() {
        isRollingForward = false;
        StopOverride();
    }

    public void BackwardRollStart() {
        OverrideVelocity(Vector2.zero);
    }

    public void BackwardRollMovement() {
        isRollingBackward = true;
        Vector2 direction = new Vector2(isFacingRight ? -rollSpeed : rollSpeed, 0);
        OverrideVelocity(direction);
    }

    public void BackwardRollEnd() {
        isRollingBackward = false;
        StopOverride();
    }

    //ICharacter Implement Functions

    public override void DisableMovement() {
        base.DisableMovement();
        animator.speed = 0;
    }

    public override void EnableMovement() {
        base.EnableMovement();
        animator.speed = 1;
    }

    public void ActionInput(ActionInput actionInput) {
        actionInputQueue = actionInput;
    }

    public void ShieldHold(bool holdingShield) {
        isShielding = holdingShield;
    }

    public void Tumble() {
        isTumbling = true;
    }

    //Overrides

    protected override void Jump(float jumpHeight) {
        base.Jump(jumpHeight);
        animator.SetTrigger("Jump");
    }

    protected override void DoubleJump(float jumpHeight) {
        base.DoubleJump(jumpHeight);
        animator.SetTrigger("DoubleJump");
    }
}
