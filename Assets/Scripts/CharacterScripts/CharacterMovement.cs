using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    protected Rigidbody2D rb;
    protected CapsuleCollider2D col;
    protected CharacterStats stats;

    public CharacterStats Stats {get {return stats;} set {stats = value;}}

    public bool movementEnabled;
    public Color debugColor;
    public LayerMask groundLayerMask;

    public IController controller;
    public Vector2 mainJoystick;
    public BoolStat ignoreInput;
    public BoolStat ignoreMainJoystick;

    [SerializeField] protected Vector2 velocity;
    public Vector2 Velocity {get {return velocity;} set {velocity = value;}}
    public bool overrideVelocity;
    public Vector2 overridingVelocity;

    [SerializeField] protected bool isFacingRight;
    public bool IsFacingRight {get {return isFacingRight;} set {isFacingRight = value;}}
    public GameObject[] flipTargets;
    [SerializeField] protected bool isGrounded;
    public bool IsGrounded {get {return isGrounded;} set {isGrounded = value;}}

    //Ground Movement Variables
    public float maxWalkSpeed;
    public float walkAccelerationRate;
    public float groundDecelerationRate;
    public float initialDashSpeed;
    public float maxDashSpeed;
    public float dashAccelerationRate;
    public float decelerateAxisThreshold;
    public bool preventWalkOffLedge;
    public float preventWalkOffLedgeThreshold;
    protected bool _preventWalkOffLedge;
    public float ledgeDetectionRatio;
    public bool isOnLeftLedge;
    public bool isOnRightLedge;
    protected bool isWalking;
    protected bool isDashing;
    protected bool isCrouching;
    protected GameObject currentPlatform;

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
    protected bool isJumpSquatting;
    public float shortHopHeight;
    public float fullHopHeight;
    public float doubleJumpHeight;
    public int doubleJumpCount;
    protected int doubleJumpLeft;
    protected bool jumpBuffered;
    protected bool jumpButtonHeld;
    public int jumpBufferFrame;
    protected Coroutine JumpBufferCoroutine;
    public bool canJumpChangeDirection;
    
    // Start is called before the first frame update
    void Awake()
    {
        Initialize();
    }

    protected virtual void Initialize() {
        InitializeComponents();
        InitializeStats();
        InitializeVariables();
    }

    protected virtual void InitializeComponents() {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CapsuleCollider2D>();
        stats = new CharacterStats();
    }

    protected virtual void InitializeStats() {
        stats.MaxWalkSpeed = maxWalkSpeed;
        stats.WalkAccelerationRate = walkAccelerationRate;
        stats.GroundDecelerationRate = groundDecelerationRate;
        stats.InitialDashSpeed = initialDashSpeed;
        stats.MaxDashSpeed = maxDashSpeed;
        stats.DashAccelerationRate = dashAccelerationRate;
        stats.MaxAirSpeed = maxAirSpeed;
        stats.AirAccelerationRate = airAccelerationRate;
        stats.AirDecelerationRate = airDecelerationRate;
        stats.Gravity = gravity;
        stats.MaxFallSpeed = maxFallSpeed;
        stats.MaxFastFallSpeed = maxFastFallSpeed;
        stats.ShortHopHeight = shortHopHeight;
        stats.FullHopHeight = fullHopHeight;
        stats.DoubleJumpHeight = doubleJumpHeight;
    }

    protected virtual void InitializeVariables() {
        movementEnabled = true;
        enableAirDeceleration = true;
        doubleJumpLeft = doubleJumpCount;

        ignoreInput = new BoolStat(false);
        ignoreMainJoystick = new BoolStat(false);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateInput();
    }

    protected virtual void UpdateInput() {
        
    }

    IEnumerator JumpBuffer(int jumpBufferFrame) {
        int jumpBufferFrameLeft = 0;
        while (jumpBufferFrameLeft < jumpBufferFrame) {
            jumpBufferFrameLeft++;
            yield return null;
        }
        jumpBuffered = false;
    }

    IEnumerator JumpSquat(int jumpSquatFrame) {
        int jumpSquatFrameLeft = 0;
        while (jumpSquatFrameLeft < jumpSquatFrame) {
            jumpSquatFrameLeft++;
            yield return null;
        }
        isJumpSquatting = false;
        Jump(jumpButtonHeld ? Stats.FullHopHeight : Stats.ShortHopHeight);
    }

    void FixedUpdate() {
        Tick();
    }

    protected virtual void Tick() {
        if (movementEnabled) {
            GroundCheck();
            if (overrideVelocity) {
                UpdateOverridingVelocity();
            }
            else {
                UpdatePhysics();
                UpdateMovement();
            }
            LedgeWalkOffCheck();
            PlatformCheck();
            UpdateFacingDirection();
            FlipCheck();
            ApplyVelocity();
        }
    }

    protected virtual void ApplyVelocity() {
        rb.MovePosition(rb.position + Velocity * Time.fixedDeltaTime);
        //rb.velocity = velocity;
        //Debug.DrawRay(GetFeetPosition(), -Vector2.up * 0.1f, debugColor);
        //Debug.DrawRay(GetFeetPosition(velocity * Time.fixedDeltaTime), -Vector2.up, debugColor);
    }

    protected virtual void UpdateOverridingVelocity() {
        Velocity = overridingVelocity;
    }

    protected virtual void UpdatePhysics() {
        //Ground Check Update
        if (IsGrounded) {
            //Velocity = new Vector2 (Velocity.x, 0.0f);
        }
        else {
            //Falling Speed Update
            UpdateFallSpeed();
        }
    }

    protected virtual void UpdateFallSpeed() {
        if (isFastFalling) {
            Velocity = new Vector2 (Velocity.x, -Stats.MaxFastFallSpeed);
        }
        else {
            Velocity = new Vector2 (Velocity.x, Mathf.Max(GetTargetVelocity(Velocity.y, -Stats.MaxFallSpeed, Stats.Gravity)));
        }
    }

    protected virtual void UpdateMovement() {
        if (ignoreInput.stat || ignoreMainJoystick.stat) {
            mainJoystick = Vector2.zero;
        }
        //Ground Movement
        if (IsGrounded) {
            UpdateGroundMovement();
            UpdateGroundJumpMovement();
        }
        //Air Movement
        else {
            UpdateAirMovement();
            UpdateAirJumpMovement();
            UpdateFallThroughMovement();
        }
    }

    protected virtual void UpdateGroundMovement() {
        //Horizontal Ground Movement
        float targetGroundVelocity;
        isWalking = Mathf.Abs(mainJoystick.x) > 0 && !isDashing && !isCrouching;
        isDashing = Mathf.Abs(mainJoystick.x) > 0 && !isWalking && !isCrouching;
        if (isCrouching) { //Crouching
            targetGroundVelocity = GetTargetVelocity(Velocity.x, 0.0f, Stats.GroundDecelerationRate);
        }
        else {
            if (isDashing) { //Dashing
                targetGroundVelocity = mainJoystick.x > 0 ?
                Mathf.Max(Stats.InitialDashSpeed, GetTargetVelocity(Velocity.x, mainJoystick.x * Stats.MaxDashSpeed, Stats.DashAccelerationRate)) :
                Mathf.Min(-Stats.InitialDashSpeed, GetTargetVelocity(Velocity.x, mainJoystick.x * Stats.MaxDashSpeed, Stats.DashAccelerationRate));
            }
            else { //Walking
                targetGroundVelocity = Mathf.Abs(mainJoystick.x) < decelerateAxisThreshold ?
                    GetTargetVelocity(Velocity.x, 0.0f, Stats.GroundDecelerationRate)
                    : GetTargetVelocity(Velocity.x, mainJoystick.x * Stats.MaxWalkSpeed, Stats.WalkAccelerationRate);
            }
        }
        Velocity = new Vector2 (targetGroundVelocity, Velocity.y);
        if (Mathf.Abs(mainJoystick.x) < preventWalkOffLedgeThreshold) _preventWalkOffLedge = true;
        else _preventWalkOffLedge = false;
    }

    protected virtual void UpdateGroundJumpMovement() {
        if (ignoreInput.stat) return;
        //Consume Jump Buffer and start jumpsquatting. Character will jump after the jumpsquat. 
        if (jumpBuffered) {
            isJumpSquatting = true;
            jumpBuffered = false;
            StartCoroutine(JumpSquat(jumpSquatFrame));
        }
    }

    protected virtual void UpdateAirMovement() {
        //Horizontal Air Movement
        float targetAirVelocity;
        if (enableAirDeceleration) {
            targetAirVelocity = Mathf.Abs(mainJoystick.x) < decelerateAxisThreshold ?
                GetTargetVelocity(Velocity.x, 0.0f, Stats.AirDecelerationRate)
                : GetTargetVelocity(Velocity.x, mainJoystick.x * Stats.MaxAirSpeed, Stats.AirAccelerationRate);
        }
        else {
            targetAirVelocity = Velocity.x;
        }
        Velocity = new Vector2(targetAirVelocity, Velocity.y);
        _preventWalkOffLedge = false;
    }

    protected virtual void UpdateAirJumpMovement() {
        if (ignoreInput.stat) return;
        //Consume JumpBuffer and Double Jump
        if (jumpBuffered) {
            if (doubleJumpLeft > 0) {
                DoubleJump(Stats.DoubleJumpHeight);
                doubleJumpLeft--;
                jumpBuffered = false;
            }
        }
    }

    protected virtual void UpdateFallThroughMovement() {
        //Falling Through
        if (isFallingThrough) {
            fallingThroughFrameLeft--;
        }
    }

    protected virtual void LedgeWalkOffCheck() {
        //Prevent Walk Off Ledge Update
        if (preventWalkOffLedge || _preventWalkOffLedge) {
            LedgeCheck();
        }
    }

    protected virtual void UpdateFacingDirection() {
        if (ignoreInput.stat) return;
        if (IsGrounded) {
            if (mainJoystick.x > 0) {
                IsFacingRight = true;
            }
            else if (mainJoystick.x < 0) {
                IsFacingRight = false;
            }
        }
    }

    //ICharacter Implement Functions

    public void OverrideVelocity(Vector2 targetVelocity) {
        overrideVelocity = true;
        overridingVelocity = targetVelocity;
    }
    
    public void StopOverride() {
        overrideVelocity = false;
    }

    public virtual void Move(Vector2 input) {
        if (ignoreMainJoystick.stat) return;
        mainJoystick = input;
    }

    public virtual void Dash() {
        if (ignoreInput.stat) return;
        isDashing = true;
    }

    public virtual void StopDash() {
        isDashing = false;
    }

    public virtual void Jump() {
        //Buffer jump
        jumpBuffered = true;
        StartCoroutine(JumpBuffer(jumpBufferFrame));
    }

    public void JumpHold(bool holdingJump) {
        jumpButtonHeld = holdingJump;
    }

    public virtual void Crouch() {
        if (ignoreInput.stat) return;
        isCrouching = true;
    }

    public void UnCrouch() {
        isCrouching = false;
    }

    public virtual void FallThrough() {
        if (ignoreInput.stat) return;
        if (!isFallingThrough) {
            if (!IsGrounded) {
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

    public virtual void FastFall() {
        if (ignoreInput.stat) return;
        isFastFalling = !IsGrounded && Velocity.y < 0;
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
        ignoreInput.Switch(ignore);
        mainJoystick = Vector2.zero;
        Debug.Log("IgnoreInput: " + ignoreInput.stat + ", Count: " + ignoreInput.setCount);
    }

    public void IgnoreMainJoystick(bool ignore) {
        ignoreMainJoystick.Switch(ignore);
        mainJoystick = Vector2.zero;
        Debug.Log("ignoreMainJoystick: " + ignoreMainJoystick.stat + ", Count: " + ignoreMainJoystick.setCount);
    }

    public void PreventWalkOffLedge() {
        preventWalkOffLedge = true;
    }

    public void UnPreventWalkOffLedge() {
        preventWalkOffLedge = false;
    }


    //Character Movement Functions

    protected void FlipCheck() {
            foreach(GameObject target in flipTargets) {
                int modifier = !(IsFacingRight ^ target.transform.localScale.x > 0) ? 1 : -1;
                target.transform.localPosition = Vector3.Scale(target.transform.localPosition, new Vector3(modifier, 1, 1));
                target.transform.localScale = Vector3.Scale(target.transform.localScale, new Vector3(modifier, 1, 1));
            }
    }

    public void Flip() {
        IsFacingRight = !IsFacingRight;
        FlipCheck();
    }

    public void Face(GameObject target) {
        IsFacingRight = target.transform.position.x > this.gameObject.transform.position.x;
        FlipCheck();
    }

    public float GetTargetVelocity(float currentVelocity, float targetVelocity, float accelerationRate) {
        int modifier = currentVelocity > targetVelocity ? -1 : 1;
        float acceleration = accelerationRate * Time.fixedDeltaTime;
        if (Mathf.Abs(currentVelocity + acceleration * modifier - targetVelocity) > acceleration) {
            return currentVelocity + acceleration * modifier;
        }
        else {
            return targetVelocity;
        }
    }

    protected void RestoreDoubleJumps() {
        //Restore Double Jumps
        doubleJumpLeft = doubleJumpCount;
    }

    protected virtual void GroundCheck() {
        //Debug.Log(GetFeetPosition());
        if (Velocity.y > 0) {
            IsGrounded = false;
            return;
        }
        RaycastHit2D centerRay = Physics2D.Raycast(GetFeetPosition(), -Vector2.up, 0.1f, groundLayerMask);
        if (centerRay && !IsGrounded) OnLand();
        IsGrounded = centerRay;
    }

    protected virtual void OnLand() {
        RestoreDoubleJumps();
        isFastFalling = false;
    }

    protected void PlatformCheck() {
        //if (rb.velocity.y > 0) return;
        RaycastHit2D centerRay = Physics2D.Raycast(GetFeetPosition(), -Vector2.up, 50, groundLayerMask);
        //If the platform the raycast is looking at changes, ignore collision with the old platform and collide with the new one
        if (!centerRay && currentPlatform != null) {
            currentPlatform.GetComponent<IGround>().IgnoreCollision(col, true);
            currentPlatform = null;
        }
        else if (centerRay && currentPlatform == null) {
            centerRay.collider.gameObject.GetComponent<IGround>().IgnoreCollision(col, false);
            currentPlatform = centerRay.collider.gameObject;
        }
        else if (centerRay && currentPlatform != null) {
            if (!currentPlatform.Equals(centerRay.collider.gameObject)) {
                currentPlatform.GetComponent<IGround>().IgnoreCollision(col, true);
                centerRay.collider.gameObject.GetComponent<IGround>().IgnoreCollision(col, false);
                currentPlatform = centerRay.collider.gameObject;
            }
        }
        //Debug.Log(currentPlatform != null? currentPlatform.name : "None");
    }

    protected void LedgeCheck() {
        if (!IsGrounded) return;
        RaycastHit2D centerRay = Physics2D.Raycast(GetFeetPosition(Velocity * Time.fixedDeltaTime), -Vector2.up, 1.0f, groundLayerMask);
        //If the platform the raycast is looking at is different, stop movement off the ledge
        if (!centerRay && currentPlatform != null) {
            Velocity = Vector2.zero;
        }
        else if (centerRay && currentPlatform != null) {
            if (!currentPlatform.Equals(centerRay.collider.gameObject)) {
                Velocity = Vector2.zero;
            }
        }
    }

    protected Vector2 GetFeetPosition() {
        return new Vector2(rb.position.x, rb.position.y + col.offset.y - col.size.y / 2.0f);
    }

    protected Vector2 GetFeetPosition(Vector2 offset) {
        return new Vector2(rb.position.x + offset.x, rb.position.y + col.offset.y - col.size.y / 2.0f + offset.y);
    }

    protected virtual void Jump(float jumpHeight) {
        IsGrounded = false;
        _preventWalkOffLedge = false;
        if (canJumpChangeDirection) {
            Velocity = new Vector2 (mainJoystick.x * Stats.MaxAirSpeed, Mathf.Sqrt(2.0f * Stats.Gravity * jumpHeight));
        }
        else {
            Velocity = new Vector2 (Velocity.x, Mathf.Sqrt(2.0f * Stats.Gravity * jumpHeight));
        }
        if (mainJoystick.x > 0) IsFacingRight = true;
        if (mainJoystick.x < 0) IsFacingRight = false;
        FlipCheck();
    }

    protected virtual void DoubleJump(float jumpHeight) {
        IsGrounded = false;
        _preventWalkOffLedge = false;
        isFastFalling = false;
        if (canJumpChangeDirection) {
            Velocity = new Vector2 (mainJoystick.x * Stats.MaxAirSpeed, Mathf.Sqrt(2.0f * Stats.Gravity * jumpHeight));
        }
        else {
            Velocity = new Vector2 (Velocity.x, Mathf.Sqrt(2.0f * Stats.Gravity * jumpHeight));
        }
    }

    //Collision Functions

    protected virtual void OnCollisionEnter2D(Collision2D collision) {
        if (collision.collider.gameObject.tag == "Ground") {
            //Debug.Log("Collided with Ground: " + collision.collider.gameObject.name);
            if (Velocity.y < 0) {
                Velocity = new Vector2(Velocity.x, 0.0f);
            }
        }
        if (collision.collider.gameObject.tag == "Platform") {
            //Debug.Log("Collided with Platform: " + collision.collider.gameObject.name);
            if (Velocity.y < 0) {
                Velocity = new Vector2(Velocity.x, 0.0f);
            }
        }
    }

    protected virtual void OnCollisionStay2D(Collision2D collision) {

    }

    protected virtual void OnCollisionExit2D(Collision2D collision) {

    }

}
