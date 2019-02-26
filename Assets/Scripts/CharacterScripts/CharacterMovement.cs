using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    protected Rigidbody2D rb;
    protected Transform trans;
    protected CapsuleCollider2D col;
    public bool movementEnabled;
    public Color debugColor;
    public LayerMask groundLayerMask;

    public IController controller;
    public Vector2 mainJoystick;
    public bool ignoreInput;

    public Vector2 velocity;
    public bool overrideVelocity;
    public Vector2 overridingVelocity;
    
    public bool isBusy;
    public bool isFacingRight;
    public GameObject[] flipTargets;
    public bool isGrounded;

    //Ground Movement Variables
    public float maxWalkSpeed;
    public float walkAccelerationRate;
    public float groundDecelerationRate;
    public float initialDashSpeed;
    public float maxDashSpeed;
    public float dashAccelerationRate;
    public float decelerateAxisThreshold;
    public bool preventWalkOffLedge;
    public float ledgeDetectionRatio;
    public bool isOnLeftLedge;
    public bool isOnRightLedge;
    protected bool isWalking;
    protected bool isDashing;
    protected bool isCrouching;

    //Air Movement Variables
    public float maxAirSpeed;
    public float airAccelerationRate;
    public float airDecelerationRate;
    public bool enableAirDeceleration;
    public float gravity;
    public float maxFallSpeed;
    public bool isFastFalling;
    public float maxFastFallSpeed;
    public bool isFallingThrough;
    public float fallingThroughMinFrame;
    float fallingThroughFrameLeft;

    //Jump Variables
    public int jumpSquatFrame;
    protected int jumpFrameLeft;
    protected bool isJumpSquatting;
    public float shortHopHeight;
    public float fullHopHeight;
    public float doubleJumpHeight;
    public int doubleJumpCount;
    protected int doubleJumpLeft;
    protected bool jumpBuffered;
    protected bool jumpButtonHeld;
    public int jumpBufferFrame;
    protected int jumpBufferFrameLeft;
    public bool canJumpChangeDirection;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        trans = GetComponent<Transform>();
        col = GetComponent<CapsuleCollider2D>();

        movementEnabled = true;
        enableAirDeceleration = true;
        jumpFrameLeft = jumpSquatFrame;
        jumpBufferFrameLeft = jumpBufferFrame;
        doubleJumpLeft = doubleJumpCount;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateInput();
    }

    void FixedUpdate() {
        if (movementEnabled) {
            if (overrideVelocity) {
                velocity = overridingVelocity;
                rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
                overrideVelocity = false;
            }
            else {
                UpdatePhysics();
                UpdateMovement();
            }
            PlatformCheck();
            UpdateFacingDirection();
            FlipCheck();
        }
    }

    protected virtual void UpdateInput() {
        if (jumpBuffered) {
            jumpBufferFrameLeft--;
            if (jumpBufferFrameLeft < 0) {
                jumpBufferFrameLeft = jumpBufferFrame;
                jumpBuffered = false;
            }
        }
    }

    protected void UpdatePhysics() {
        //Ground Check Update
        isGrounded = GroundCheck();
        if (isGrounded) {
            //Restore Double Jumps
            doubleJumpLeft = doubleJumpCount;

            isFastFalling = false;
        }

        //Falling Speed Update
        if (isFastFalling) {
            velocity = new Vector2 (velocity.x, -maxFastFallSpeed);
        }
        else {
            velocity = new Vector2 (velocity.x, Mathf.Max(velocity.y - gravity * Time.fixedDeltaTime, -maxFallSpeed));
        }
        
        
    }

    protected void UpdateMovement() {
        if (ignoreInput) {
            mainJoystick = Vector2.zero;
        }
        //Ground Movement
        if (isGrounded) {
            //Ignore Joystick Input if busy
            if (isBusy) {
                mainJoystick = Vector2.zero;
            }
            //Horizontal Ground Movement
            float targetGroundVelocity;
            isWalking = Mathf.Abs(mainJoystick.x) > 0 && !isDashing && !isCrouching;
            isDashing = Mathf.Abs(mainJoystick.x) > 0 && !isWalking && !isCrouching;
            if (isCrouching) { //Crouching
                targetGroundVelocity = GetTargetVelocity(0.0f, groundDecelerationRate);
            }
            else {
                if (isDashing) { //Dashing
                    targetGroundVelocity = mainJoystick.x > 0 ? 
                    Mathf.Max(initialDashSpeed, GetTargetVelocity(mainJoystick.x * maxDashSpeed, dashAccelerationRate)) : 
                    Mathf.Min(-initialDashSpeed, GetTargetVelocity(mainJoystick.x * maxDashSpeed, dashAccelerationRate));
                }
                else { //Walking
                    targetGroundVelocity = Mathf.Abs(mainJoystick.x) < decelerateAxisThreshold ? 
                        GetTargetVelocity(0.0f, groundDecelerationRate) 
                        : GetTargetVelocity(mainJoystick.x * maxWalkSpeed, walkAccelerationRate);
                }
            }
            
            
            velocity = new Vector2 (targetGroundVelocity, velocity.y);
            
            //Jump
            if (jumpBuffered && !isBusy) {
                isJumpSquatting = true;
                jumpBufferFrameLeft = jumpBufferFrame;
                jumpBuffered = false;
            }
            if (isJumpSquatting) {
                jumpFrameLeft--;
                if (jumpFrameLeft < 0) {
                    Jump(jumpButtonHeld ? fullHopHeight : shortHopHeight);
                    jumpFrameLeft = jumpSquatFrame;
                    isJumpSquatting = false;
                }
            }
        }
        //Air Movement
        else {
            //Horizontal Air Movement
            float targetAirVelocity;
            if (enableAirDeceleration) {
                targetAirVelocity = Mathf.Abs(mainJoystick.x) < decelerateAxisThreshold ?
                    GetTargetVelocity(0.0f, airDecelerationRate)
                    : GetTargetVelocity(mainJoystick.x * maxAirSpeed, airAccelerationRate);
            }
            else {
                targetAirVelocity = rb.velocity.x;
            }
            velocity = new Vector2 (targetAirVelocity, velocity.y);
            
            //Double Jump
            if (jumpBuffered && !isBusy) {
                if (doubleJumpLeft > 0) {
                    DoubleJump(doubleJumpHeight);
                    doubleJumpLeft--;
                    jumpBufferFrameLeft = jumpBufferFrame;
                    jumpBuffered = false;
                }
            }

            //Falling Through
            if (isFallingThrough) {
                fallingThroughFrameLeft--;
            }
        }
        rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
    }

    protected void UpdateFacingDirection() {
        if (isBusy) return;
        if (isGrounded) {
            if (velocity.x > 0) {
                isFacingRight = true;
            }
            else if (velocity.x < 0) {
                isFacingRight = false;
            }
        }
    }

    //ICharacter Implement Functions

    public bool IsGrounded() {
        return isGrounded;
    }

    public bool IsFacingRight() {
        return isFacingRight;
    }

    public Vector2 GetVelocity() {
        return velocity;
    }

    public void SetVelocity(Vector2 velocity) {
        overrideVelocity = true;
        overridingVelocity = velocity;
    }
    
    public void Move(Vector2 input) {
        mainJoystick = input;
    }

    public void Dash() {
        if (isBusy) {
            return;
        }
        isDashing = true;
    }

    public void StopDash() {
        isDashing = false;
    }

    public void Jump() {
        //Buffer jump
        jumpBuffered = true;
    }

    public void JumpHold(bool holdingJump) {
        jumpButtonHeld = holdingJump;
    }

    public void Crouch() {
        if (isBusy) {
            return;
        }
        isCrouching = true;
    }

    public void UnCrouch() {
        isCrouching = false;
    }

    public void FallThrough() {
        if (isBusy) {
            return;
        }
        if (!isFallingThrough) {
            if (!isGrounded) {
                fallingThroughFrameLeft = 0;
            }
            isFallingThrough = true;
            this.gameObject.layer = LayerMask.NameToLayer("FallingThroughEntity");
            groundLayerMask = LayerMask.GetMask("Ground");
        }
    }

    public void StopFallThrough() {
        if (fallingThroughFrameLeft > 0) return;
        if (isFallingThrough) {
            isFallingThrough = false;
            this.gameObject.layer = LayerMask.NameToLayer("Entity");
            groundLayerMask = LayerMask.GetMask("Ground", "OneWayPlatform");
            fallingThroughFrameLeft = fallingThroughMinFrame;
        }
        
    }

    public void FastFall() {
        isFastFalling = !isGrounded && velocity.y < 0;
    }

    public void EnableAirDeceleration(bool enable) {
        enableAirDeceleration = enable;
    }

    public virtual void DisableMovement() {
        movementEnabled = false;
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;
        isFastFalling = false;
    }

    public virtual void EnableMovement() {
        movementEnabled = true;
        rb.velocity = Vector2.zero;
        rb.isKinematic = false;
    }

    public void IgnoreInput(bool ignore) {
        ignoreInput = ignore;
    }


    //Character Movement Functions

    protected void FlipCheck() {
        if (isFacingRight) {
            foreach(GameObject target in flipTargets) {
                target.transform.localScale = new Vector3(1, 1, 1);
            }
            //trans.rotation = Quaternion.Euler(0, 0, 0);
            //Debug.Log("Right");
        }
        else {
            foreach(GameObject target in flipTargets) {
                target.transform.localScale = new Vector3(-1, 1, 1);
            }
            //trans.rotation = Quaternion.Euler(0, 180, 0);
            //Debug.Log("Left");
        }
    }

    public void Flip() {
        isFacingRight = !isFacingRight;
        FlipCheck();
    }

    protected float GetTargetVelocity(float targetVelocity, float accelerationRate) {
        int modifier = velocity.x > targetVelocity ? -1 : 1;
        float acceleration = accelerationRate * Time.fixedDeltaTime;
        if (Mathf.Abs(velocity.x + acceleration * modifier - targetVelocity) > acceleration) {
            return velocity.x + acceleration * modifier;
        }
        else {
            return targetVelocity;
        }
    }

    protected bool GroundCheck() {
        //Debug.Log(GetFeetPosition());
        if (velocity.y > 0) return false;
        RaycastHit2D centerRay = Physics2D.Raycast(GetFeetPosition(), -Vector2.up, 0.1f, groundLayerMask);
        RaycastHit2D leftRay = Physics2D.Raycast(GetFeetPosition(-col.size.x / 2.0f, 0), -Vector2.up, 0.1f, groundLayerMask);
        RaycastHit2D rightRay = Physics2D.Raycast(GetFeetPosition(col.size.x / 2.0f, 0), -Vector2.up, 0.1f, groundLayerMask);
        //return centerRay && leftRay && rightRay;
        return centerRay;
    }

    protected void PlatformCheck() {
        //if (rb.velocity.y > 0) return;
        RaycastHit2D centerRay = Physics2D.Raycast(GetFeetPosition(), -Vector2.up, 50, groundLayerMask);
        RaycastHit2D leftRay = Physics2D.Raycast(GetFeetPosition(-col.size.x / 2.0f, 0), -Vector2.up, 50, groundLayerMask);
        RaycastHit2D rightRay = Physics2D.Raycast(GetFeetPosition(col.size.x / 2.0f, 0), -Vector2.up, 50, groundLayerMask);
        if (!(centerRay && leftRay && rightRay)) return;
        if (centerRay.collider.gameObject == leftRay.collider.gameObject && centerRay.collider.gameObject == rightRay.collider.gameObject) {
            if (centerRay.collider.gameObject.tag == "Platform") {
                if (isFallingThrough) {
                    centerRay.collider.gameObject.GetComponent<IPlatform>().IgnorePlatformCollision(col, true);
                }
                else {
                    centerRay.collider.gameObject.GetComponent<IPlatform>().IgnorePlatformCollision(col, false);
                }
                if (preventWalkOffLedge && !isDashing && isGrounded) {
                    centerRay.collider.gameObject.GetComponent<IPlatform>().IgnoreEdgeCollision(col, false);
                }
                else {
                    centerRay.collider.gameObject.GetComponent<IPlatform>().IgnoreEdgeCollision(col, true);
                }
            }
            else if (centerRay.collider.gameObject.tag == "Ground") {
                centerRay.collider.gameObject.GetComponent<IPlatform>().IgnorePlatformCollision(col, false);
                if (preventWalkOffLedge && !isDashing && isGrounded) {
                    centerRay.collider.gameObject.GetComponent<IPlatform>().IgnoreEdgeCollision(col, false);
                }
                else {
                    centerRay.collider.gameObject.GetComponent<IPlatform>().IgnoreEdgeCollision(col, true);
                }
            }
        }
        else {
            if (centerRay.collider.gameObject.tag == "Platform") {
                centerRay.collider.gameObject.GetComponent<IPlatform>().IgnoreAllCollision(col, true);
            }
            if (leftRay.collider.gameObject.tag == "Platform") {
                leftRay.collider.gameObject.GetComponent<IPlatform>().IgnoreAllCollision(col, true);
            }
            if (rightRay.collider.gameObject.tag == "Platform") {
                rightRay.collider.gameObject.GetComponent<IPlatform>().IgnoreAllCollision(col, true);
            }
        }
    }

    protected void LedgeCheck(float offset) {
        if (!isGrounded) return;
        Debug.Log("Ledge Checking");
        RaycastHit2D leftRay = Physics2D.Raycast(GetFeetPosition((-col.size.x / 2.0f) * ledgeDetectionRatio + offset, 0), -Vector2.up, 0.1f, groundLayerMask);
        RaycastHit2D rightRay = Physics2D.Raycast(GetFeetPosition((col.size.x / 2.0f) * ledgeDetectionRatio + offset, 0), -Vector2.up, 0.1f, groundLayerMask);
        isOnLeftLedge = !leftRay;
        isOnRightLedge = !rightRay;
    }

    protected Vector2 GetFeetPosition() {
        return new Vector2(trans.position.x, trans.position.y + col.offset.y - col.size.y / 2.0f);
    }

    protected Vector2 GetFeetPosition(float offsetX, float offsetY) {
        return new Vector2(trans.position.x + offsetX, trans.position.y + col.offset.y - col.size.y / 2.0f + offsetY);
    }

    protected virtual void Jump(float jumpHeight) {
        isGrounded = false;
        if (canJumpChangeDirection) {
            velocity = new Vector2 (mainJoystick.x * maxAirSpeed, Mathf.Sqrt(2.0f * gravity * jumpHeight));
        }
        else {
            velocity = new Vector2 (rb.velocity.x, Mathf.Sqrt(2.0f * gravity * jumpHeight));
        }
        if (mainJoystick.x > 0) isFacingRight = true;
        if (mainJoystick.x < 0) isFacingRight = false;
        FlipCheck();
    }

    protected virtual void DoubleJump(float jumpHeight) {
        isGrounded = false;
        isFastFalling = false;
        if (canJumpChangeDirection) {
            velocity = new Vector2 (mainJoystick.x * maxAirSpeed, Mathf.Sqrt(2.0f * gravity * jumpHeight));
        }
        else {
            velocity = new Vector2 (rb.velocity.x, Mathf.Sqrt(2.0f * gravity * jumpHeight));
        }
    }

}
