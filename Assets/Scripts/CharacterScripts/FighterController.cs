using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FighterController : CharacterMovement
{
    protected Animator animator;
    protected StatusManager statusManager;
    protected HurtboxManager hurtbox;
    protected EventManager eventManager;

    public GameObject collidingWall;
    protected Vector2 collisionContact;
    protected Vector2 collisionNormal;

    public bool isBusy;
    
    public bool isJabbing;
    public bool canJab;

    [SerializeField] protected bool isHelpless;
    public bool IsHelpless {get {return isHelpless;} set {isHelpless = value;}}
    protected IStatus helplessStatus;
    [SerializeField] protected bool isTumbling;
    public bool IsTumbling {get {return isTumbling;} set {isTumbling = value;}}
    protected IStatus tumblingStatus;
    [SerializeField] protected bool isTumbled;
    public bool IsTumbled {get {return isTumbled;} set {isTumbled = value;}}
    protected Timer TumbleDurationTimer;
    public const int TUMBLE_MAXIMUM_DURATION_FRAME = 180;
    [SerializeField] protected bool isTeching;
    public bool IsTeching {get {return isTeching;} set {isTeching = value;}}
    protected Timer TechWindowDurationTimer;
    public const int TECH_WINDOW_FRAME = 11;

    public bool isShielding;
    public bool isRollingForward;
    public bool isRollingBackward;
    public float rollSpeed;
    protected IStatus movementStatus;
    
    protected BoolStat canBeGrabbed;
    public bool CanBeGrabbed {get {return canBeGrabbed.stat;} set {canBeGrabbed.Switch(value);}}
    public bool isGrabbing;
    public bool isGrabbed;
    public GameObject grabbingFighter;
    public GameObject grabber;
    public Transform grabTransform;
    protected int grabDurationLeft;
    protected Coroutine GrabDurationTimer;
    public const int GRAB_MAXIMUM_DURATION_FRAME = 180;
    protected IStatus grabImmuneStatus;
    public const int GRAB_IMMUNE_FRAME = 70;

    protected BoolStat canGrabEdge;
    public bool CanGrabEdge {get {return canGrabEdge.stat;} set {canGrabEdge.Switch(value);}}
    public bool isEdgeGrabImmune;
    public bool isGrabbingEdge;
    public GameObject grabbingEdge;
    public Transform edgeGrabTransform;
    public Collider2D edgeHitbox;
    protected IStatus edgeGrabIntangibilityStatus;
    protected int edgeGrabDurationLeft;
    protected Timer EdgeGrabDurationTimer;
    public const int EDGE_GRAB_MAXIMUM_DURATION_FRAME = 300;
    protected Timer EdgeGrabImmuneTimer;
    public const int EDGE_GRAB_IMMUNE_FRAME = 15;
    
    //Action Buffer Queue Veriables
    public int actionBufferFrame;
    protected bool isActionBuffered;
    protected Coroutine ActionBufferCoroutine;
    protected ActionInput actionInputQueue;
    
    // Start is called before the first frame update
    void Awake()
    {
        Initialize();
    }

    protected override void InitializeComponents() {
        base.InitializeComponents();
        animator = GetComponent<Animator>();
        statusManager = GetComponent<StatusManager>();
        hurtbox = GetComponentInChildren<HurtboxManager>();
        eventManager = (EventManager) GameObject.FindObjectOfType(typeof(EventManager));
        eventManager.StartListeningToOnHitStunEvent(new UnityAction<IAttackHitbox, GameObject>(OnHitStun));
        eventManager.StartListeningToOnGrabEvent(new UnityAction<GameObject, GameObject>(OnGrab));
        eventManager.StartListeningToOnEdgeGrabEvent(new UnityAction<GameObject, GameObject>(OnEdgeGrab));
    }

    protected override void InitializeVariables() {
        EdgeGrabDurationTimer = new Timer(EDGE_GRAB_MAXIMUM_DURATION_FRAME, "Edge Grab Duration Timer", null, EdgeNeutralRelease);
        EdgeGrabImmuneTimer = new Timer(EDGE_GRAB_IMMUNE_FRAME, "Edge Grab Immune Timer", OnEdgeGrabImmuneStart, OnEdgeGrabImmuneStop);
        
        TumbleDurationTimer = new Timer (TUMBLE_MAXIMUM_DURATION_FRAME, "Tumble Duration Timer", null, OnTumbleDurationStop);
        TechWindowDurationTimer = new Timer(TECH_WINDOW_FRAME, "Tech Window Timer", OnTechWindowStart, OnTechWindowStop);

        canBeGrabbed = new BoolStat(true);
        grabImmuneStatus = new GrabImmuneStatus(GRAB_IMMUNE_FRAME);
        canGrabEdge = new BoolStat(false);
        //edgeGrabImmuneStatus = new EdgeGrabImmuneStatus(EDGE_GRAB_IMMUNE_FRAME);
        tumblingStatus = new TumblingStatus();
        helplessStatus = new HelplessStatus(0.7f);
        base.InitializeVariables();
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
        if (isGrabbingEdge) this.gameObject.transform.position = grabbingEdge.transform.position + this.gameObject.transform.position - edgeGrabTransform.position;
        base.Tick();
        edgeHitbox.enabled = !isEdgeGrabImmune && !isGrabbingEdge && (CanGrabEdge || (!isBusy && velocity.y < 0));
    }

    protected override void UpdateOverridingVelocity() {
        base.UpdateOverridingVelocity();
        if (collidingWall != null) {
            //Transfer velocity along the wall surface if the fighter is moving through forced movement
            if (overrideVelocity) {
                //If the velocity is not moving into the surface, return
                if (Vector2.Angle(Velocity, collisionNormal) <= 90.0f) return;
                Vector3 transferDirection = Vector3.ProjectOnPlane(Velocity, new Vector3(collisionNormal.x, collisionNormal.y, 0));
                Vector3 transferVelocity = transferDirection.normalized * Velocity.magnitude;
                Velocity = transferVelocity;
                Debug.DrawRay(collisionContact, transferDirection.normalized, Color.green);
            }
        }
    }

    protected override void UpdatePhysics() {
        if (isGrabbingEdge) return;
        base.UpdatePhysics();
    }

    protected override void UpdateFallSpeed() {
        if (isFastFalling) {
            Velocity = new Vector2 (Velocity.x, -Stats.MaxFastFallSpeed);
        }
        else {
            Velocity = new Vector2 (Velocity.x, Mathf.Min(GetTargetVelocity(Velocity.y, -Stats.MaxFallSpeed, Stats.Gravity), Velocity.y));
        }
    }

    protected override void UpdateMovement() {
        if (isGrabbingEdge) return;
        base.UpdateMovement();
    }

    protected override void UpdateGroundMovement() {
        //Ignore Joystick Input if busy
        if (isBusy) {
            mainJoystick = Vector2.zero;
        }
        base.UpdateGroundMovement();
    }

    protected override void UpdateGroundJumpMovement() {
        if (isBusy) return;
        if (isShielding) return;
        if (isGrabbing) return;
        if (isGrabbed) return;
        base.UpdateGroundJumpMovement();
    }

    protected override void UpdateAirJumpMovement() {
        if (isBusy) return;
        base.UpdateAirJumpMovement();
    }

    protected override void UpdateFacingDirection() {
        if (isBusy) return;
        if (isShielding) return;
        if (isGrabbing) return;
        if (isGrabbed) return;
        base.UpdateFacingDirection();
    }

    protected override void UpdateInput() {
        base.UpdateInput();
        //if (isShielding) IgnoreMainJoystick(true);
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
            }
        }
        //Debug.Log(actionInputQueue);
    }

    IEnumerator ActionBuffer(int actionBufferFrame) {
        //Debug.Log("Action Buffer Started");
        int actionBufferFrameLeft = 0;
        isActionBuffered = true;
        while (actionBufferFrameLeft < actionBufferFrame) {
            actionBufferFrameLeft++;
            //Debug.Log("Action Buffering: Frame " + actionBufferFrameLeft);
            yield return null;
        }
        isActionBuffered = false;
        actionInputQueue = null;
        //Debug.Log("Action Buffer Done");
    }

    protected virtual void UpdateAnimation() {
        animator.SetBool("isBusy", isBusy);
        animator.SetBool("isGrounded", IsGrounded);
        animator.SetBool("isJabbing", isJabbing);
        animator.SetBool("isShielding", isShielding);
        animator.SetBool("isGrabbing", isGrabbing);
        animator.SetBool("isGrabbed", isGrabbed);
        animator.SetBool("isHelpless", IsHelpless);
        animator.SetBool("isTumbling", IsTumbling);
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
        //If Ignore Input, return
        if (ignoreInput.stat) return;
        //If grabbed, consume action input into mashing
        if (isGrabbed) {
            MashFromGrab();
            return;
        }
        //Attack Actions
        switch (actionInputQueue.inputType) {
        case InputType.Attack:
            ProcessAttackInput();
            break;
        //Special Actions
        case InputType.Special:
            ProcessSpecialInput();
            break;
        //Shield Actions
        case InputType.Shield:
            ProcessShieldInput();
            break;
        //Grab Actions
        case InputType.Grab:
            ProcessGrabInput();
            break;
        //Direction Input Actions
        case InputType.Direction:
            ProcessDirectionInput();
            break;
        }
    }

    protected virtual void IgnoreActionInput() {
        ClearActionBuffer();
    }

    protected virtual void ProcessAttackInput() {
        //Cannot attack out of shield
        if (isShielding) {
            IgnoreActionInput();
            return;
        }
        //Grab Pummel
        if (isGrabbing) {
            Pummel();
            return;
        }
        //If the fighter is grabbing onto ledge, perform getup attack
        if (isGrabbingEdge) {
            animator.SetTrigger("EdgeGetupAttack");
            TimerManager.instance.StopTimer(EdgeGrabDurationTimer);
            return;
        }
        //Shake out of tumbling state when performing the attack
        if (IsTumbling) {
            statusManager.RemoveStatus(tumblingStatus);
            IsTumbling = false;
        }
        //If the fighter is tumbled on the floor, perform getup attack
        if (IsTumbled) {
            animator.SetTrigger("GetupAttack");
            TimerManager.instance.StopTimer(TumbleDurationTimer);
            UnTumble();
            return;
        }
        //Main
        if (IsGrounded) {
            ProcessGroundAttackInput();
        }
        else {
            ProcessAerialAttackInput();
        }
    }

    protected virtual void Pummel() {
        animator.SetTrigger("Pummel");
    }

    protected virtual void ProcessShieldInput() {
        if (IsTumbling) {
            if (!IsTeching) {
                IsTeching = true;
                TimerManager.instance.StartTimer(TechWindowDurationTimer);
            }
        }
        if (isGrabbing) {
            return;
        }
        if (isGrabbingEdge) {
            animator.SetTrigger("EdgeRollGetup");
            TimerManager.instance.StopTimer(EdgeGrabDurationTimer);
            return;
        }
        if (IsTumbling) {
            statusManager.RemoveStatus(tumblingStatus);
            IsTumbling = false;
        }
        if (IsTumbled) {
            animator.SetTrigger("NeutralGetup");
            TimerManager.instance.StopTimer(TumbleDurationTimer);
            UnTumble();
            return;
        }
        //Main
        if (IsGrounded) {
            ProcessGroundShieldInput();
        }
        else {
            ProcessAerialShieldInput();
        }
    }

    protected virtual void ProcessGroundAttackInput() {
        //Dash Attack
        if (isDashing) {
            animator.SetTrigger("DashAttack");
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
    }

    protected virtual void ProcessSpecialInput() {
        if (isShielding) {
            IgnoreActionInput();
            return;
        }
        if (isGrabbingEdge) return;
        if (IsTumbling) {
            statusManager.RemoveStatus(tumblingStatus);
            IsTumbling = false;
        }
        if (IsTumbled) {
            animator.SetTrigger("GetupAttack");
            TimerManager.instance.StopTimer(TumbleDurationTimer);
            UnTumble();
            return;
        }
        //Grab Pummel
        if (isGrabbing) {
            animator.SetTrigger("Pummel");
            return;
        }
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
                SideSpecial();
                return;
            case InputDirection.Left:
                isFacingRight = false;
                FlipCheck();
                SideSpecial();
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

    protected virtual void SideSpecial() {
        animator.SetTrigger("SideSpecial");
    }

    protected virtual void ProcessGroundShieldInput() {
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
    }

    protected virtual void ProcessGrabInput() {
        if (isGrabbingEdge) return;
        //Grab Pummel
        if (isGrabbing) {
            animator.SetTrigger("Pummel");
            return;
        }
        if (IsTumbling) {
            statusManager.RemoveStatus(tumblingStatus);
            IsTumbling = false;
        }
        if (IsTumbled) {
            animator.SetTrigger("GetupAttack");
            TimerManager.instance.StopTimer(TumbleDurationTimer);
            UnTumble();
            return;
        }
        if (IsGrounded) {
            ProcessGroundGrabInput();
        }
        else {
            ProcessAerialGrabInput();
        }
    }

    protected virtual void ProcessGroundGrabInput() {
        switch (actionInputQueue.inputStrength) {
        //Grab
        case InputStrength.None:
            animator.SetTrigger("Grab");
            return;
        //Directional Grab
        default:
            switch (actionInputQueue.inputDirection) {
            case InputDirection.Right:
                isFacingRight = true;
                FlipCheck();
                animator.SetTrigger("Grab");
                return;
            case InputDirection.Left:
                isFacingRight = false;
                FlipCheck();
                animator.SetTrigger("Grab");
                return;
            default:
                animator.SetTrigger("Grab");
                break;
            }
            break;
        }
    }

    protected virtual void ProcessAerialGrabInput() {
        animator.SetTrigger("Zair");
    }

    protected virtual void ProcessDirectionInput() {
        //Roll Actions
        if (isShielding) {
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
        }
        //Throw Actions
        if (isGrabbing) {
            switch (actionInputQueue.inputDirection) {
            case InputDirection.Right:
                if (isFacingRight) animator.SetTrigger("ForwardThrow");
                else animator.SetTrigger("BackThrow");
                StopCoroutine(GrabDurationTimer);
                break;
            case InputDirection.Left:
                if (isFacingRight) animator.SetTrigger("BackThrow");
                else animator.SetTrigger("ForwardThrow");
                StopCoroutine(GrabDurationTimer);
                break;
            case InputDirection.Up:
                animator.SetTrigger("UpThrow");
                StopCoroutine(GrabDurationTimer);
                break;
            case InputDirection.Down:
                animator.SetTrigger("DownThrow");
                StopCoroutine(GrabDurationTimer);
                break;
            }
        }
        //Edge Getup Actions
        if (isGrabbingEdge) {
            switch (actionInputQueue.inputDirection) {
            case InputDirection.Right:
                if (isFacingRight) animator.SetTrigger("EdgeNeutralGetup");
                else EdgeNeutralRelease();
                TimerManager.instance.StopTimer(EdgeGrabDurationTimer);
                break;
            case InputDirection.Left:
                if (isFacingRight) EdgeNeutralRelease();
                else animator.SetTrigger("EdgeNeutralGetup");
                TimerManager.instance.StopTimer(EdgeGrabDurationTimer);
                break;
            case InputDirection.Up:
                animator.SetTrigger("EdgeJumpGetup");
                TimerManager.instance.StopTimer(EdgeGrabDurationTimer);
                break;
            case InputDirection.Down:
                EdgeNeutralRelease();
                TimerManager.instance.StopTimer(EdgeGrabDurationTimer);
                break;
            }
        }
        //Getup Actions
        if (IsTumbled) {
            switch (actionInputQueue.inputDirection) {
            case InputDirection.Right:
                if (isFacingRight) animator.SetTrigger("RollForwardGetup");
                else animator.SetTrigger("RollBackwardGetup");
                TimerManager.instance.StopTimer(TumbleDurationTimer);
                UnTumble();
                break;
            case InputDirection.Left:
                if (isFacingRight) animator.SetTrigger("RollBackwardGetup");
                else animator.SetTrigger("RollForwardGetup");
                TimerManager.instance.StopTimer(TumbleDurationTimer);
                UnTumble();
                break;
            case InputDirection.Up:
                animator.SetTrigger("NeutralGetup");
                TimerManager.instance.StopTimer(TumbleDurationTimer);
                UnTumble();
                break;
            case InputDirection.Down:
                animator.SetTrigger("NeutralGetup");
                TimerManager.instance.StopTimer(TumbleDurationTimer);
                UnTumble();
                break;
            }
        }
    }

    protected virtual void EdgeNeutralRelease() {
        animator.SetTrigger("EdgeRelease"); 
        ReleaseEdge();
    }

    protected virtual void ProcessAerialAttackInput() {
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
    }

    protected virtual void ProcessAerialShieldInput() {
        switch (actionInputQueue.inputStrength) {
        //Air Dodge
        case InputStrength.None:
            animator.SetTrigger("AirDodge");
            return;
        //Directional Air Dodge
        default:
            switch (actionInputQueue.inputDirection) {
            case InputDirection.Right:
                animator.SetTrigger("AirDodge");
                return;
            case InputDirection.Left:
                animator.SetTrigger("AirDodge");
                return;
            case InputDirection.Up:
                animator.SetTrigger("AirDodge");
                return;
            case InputDirection.Down:
                animator.SetTrigger("AirDodge");
                return;
            }
            break;
        }
    }

    protected virtual void ClearActionBuffer() {
        actionInputQueue = null;
        isActionBuffered = false;
        StopCoroutine(ActionBufferCoroutine);
        //Debug.Log("Action Buffer Interrupted");
    }

    //Roll Functions

    public void ForwardRollStart() {
        movementStatus = new MovementStatus(new Vector2(isFacingRight ? rollSpeed : -rollSpeed, 0), 20);
        statusManager.AddStatus(movementStatus);
    }

    public void BackwardRollStart() {
        movementStatus = new MovementStatus(new Vector2(isFacingRight ? -rollSpeed : rollSpeed, 0), 20);
        statusManager.AddStatus(movementStatus);
    }

    //Event Functions

    public virtual void OnHitStun(IAttackHitbox hitbox, GameObject entity) {
        if (entity.Equals(this.gameObject)) {
            //Interrupt any movement status
            if (movementStatus != null) statusManager.RemoveStatus(movementStatus);
            //Remove Tumbled State
            if (IsTumbled) {
                TimerManager.instance.StopTimer(TumbleDurationTimer);
                UnTumble();
            }
            //Remove Helplessness
            if (IsHelpless) {
                IsHelpless = false; 
                statusManager.RemoveStatus(helplessStatus);
            }
            //Interrupt any grabbing state
            if (isGrabbing) GrabReleasePushed();
            //Interrupt edge grabbing
            if (isGrabbingEdge) ReleaseEdge();
        }
    }

    public virtual void OnGrab(GameObject entity, GameObject target) {
        if (entity.Equals(this.gameObject)) {
            if (!target.GetComponent<ICharacter>().CanBeGrabbed) return;
            Debug.Log(entity.name + " grabbed " + target.name);
            Grab(target);
        }
    }

    public virtual void OnEdgeGrab(GameObject entity, GameObject edge) {
        if (entity.Equals(this.gameObject)) {
            //if (!CanGrabEdge) return;
            Debug.Log(entity.name + " grabbed edge " + edge.name);
            EdgeGrab(edge);
        }
    }

    //Timer Methods

    void OnTechWindowStart() {
        IsTeching = true;
    }

    void OnTechWindowStop() {
        IsTeching = false;
    }

    void OnTumbleDurationStop() {
        animator.SetTrigger("NeutralGetup");
        UnTumble();
    }

    IEnumerator GrabDuration(int duration) {
        Debug.Log("Grab Duration Started");
        grabDurationLeft = 0;
        while (grabDurationLeft < duration) {
            grabDurationLeft++;
            Debug.Log("Grab Counting: Frame " + grabDurationLeft);
            yield return null;
        }
        Debug.Log("Grab Counting Done, Releasing...");
        GrabReleasePushed();
        animator.SetTrigger("GrabRelease");
    }

    void OnEdgeGrabImmuneStart() {
        isEdgeGrabImmune = true;
    }

    void OnEdgeGrabImmuneStop() {
        isEdgeGrabImmune = false;
    }

    //ICharacter Implement Functions

    public override void Dash() {
        if (isBusy) return;
        if (isTumbled) return;
        base.Dash();
    }

    public override void Jump() {
        if (IsTumbling) {
            statusManager.RemoveStatus(tumblingStatus);
            IsTumbling = false;
        }
        if (isTumbled) return;
        if (isGrabbingEdge) {
            if (isBusy) return;
            animator.SetTrigger("EdgeJumpGetup");
            TimerManager.instance.StopTimer(EdgeGrabDurationTimer);
            return;
        }
        base.Jump();
    }

    public override void Crouch() {
        if (isBusy) return;
        if (isTumbled) return;
        base.Crouch();
    }

    public override void FallThrough() {
        if (isBusy) return;
        if (isTumbled) return;
        base.FallThrough();
    }

    public virtual void Freeze() {
        base.DisableMovement();
        animator.speed = 0;
    }

    public virtual void UnFreeze() {
        base.EnableMovement();
        animator.speed = 1;
    }

    public void ActionInput(ActionInput actionInput) {
        if (isActionBuffered) {
            ClearActionBuffer();
        }
        actionInputQueue = actionInput;
        ActionBufferCoroutine = StartCoroutine(ActionBuffer(actionBufferFrame));
    }

    public void ShieldHold(bool holdingShield) {
        if (isTumbled) return;
        isShielding = holdingShield;
    }

    public void Helpless() {
        Debug.Log("Helpless!");
        IsHelpless = true;
        statusManager.AddStatus(helplessStatus);
    }

    public void StartTumbling() {
        IsTumbling = true;
        statusManager.AddStatus(tumblingStatus);
    }

    public void StopTumbling() {
        IsTumbling = false;
        statusManager.RemoveStatus(tumblingStatus);
    }

    public virtual void Tumble() {
        if (IsTumbling) {
            IsTumbling = false;
        }
        animator.SetTrigger("Tumble");
        IsTumbled = true;
        CanBeGrabbed = false;
        IgnoreMainJoystick(true);
        TimerManager.instance.StartTimer(TumbleDurationTimer);
    }

    public virtual void UnTumble() {
        IsTumbled = false;
        CanBeGrabbed = true;
        IgnoreMainJoystick(false);
    }

    public virtual void Grab(GameObject target) {
        CanBeGrabbed = false;
        isGrabbing = true;
        IgnoreMainJoystick(true);
        grabbingFighter = target;
        grabbingFighter.transform.position = grabTransform.position;
        grabbingFighter.transform.SetParent(grabTransform);
        
        grabbingFighter.GetComponent<ICharacter>().GetGrabbedBy(this.gameObject);
        GrabDurationTimer = StartCoroutine(GrabDuration(GRAB_MAXIMUM_DURATION_FRAME));
    }

    public virtual void GetGrabbedBy(GameObject parent) {
        if (IsTumbling) {
            statusManager.RemoveStatus(tumblingStatus);
            IsTumbling = false;
        }
        CanBeGrabbed = false;
        isGrabbed = true;
        IgnoreInput(true);
        grabber = parent;
        rb.isKinematic = true;
        Face(parent);
    }

    public virtual void GrabRelease() {
        if (grabbingFighter == null) return;
        StopCoroutine(GrabDurationTimer);
        CanBeGrabbed = true;
        isGrabbing = false;
        IgnoreMainJoystick(false);
        grabbingFighter.transform.SetParent(null);
        grabbingFighter.GetComponent<ICharacter>().FreeFromGrab(false);
        grabbingFighter = null;
    }

    public virtual void GrabReleasePushed() {
        if (grabbingFighter == null) return;
        StopCoroutine(GrabDurationTimer);
        CanBeGrabbed = true;
        isGrabbing = false;
        IgnoreMainJoystick(false);
        grabbingFighter.transform.SetParent(null);
        grabbingFighter.GetComponent<ICharacter>().FreeFromGrab(true);
        grabbingFighter = null;
    }

    public virtual void FreeFromGrab(bool pushed) {
        if (grabber == null) return;
        CanBeGrabbed = true;
        isGrabbed = false;
        IgnoreInput(false);
        grabber = null;
        rb.isKinematic = false;
        this.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
        this.gameObject.transform.localScale = Vector3.one;
        statusManager.AddStatus(grabImmuneStatus); //Grab Immunity after being releasing
        if (pushed) {
            Velocity = new Vector2(isFacingRight ? -10.0f : 10.0f, 0.0f);
            Debug.Log("Push from Grab Release");
        }
    }

    public virtual void MashFromGrab() {
        Debug.Log("Mashing!");
        grabber.GetComponent<ICharacter>().GetMashed(5);
    }

    public virtual void GetMashed(int mashAmount) {
        grabDurationLeft += 5;
        Debug.Log("Grab Counting: Frame " + grabDurationLeft);
    }

    public GameObject GetGrabbingFighter() {
        return grabbingFighter;
    }

    public GameObject GetGrabber() {
        return grabber;
    }

    public virtual void EdgeGrab(GameObject edge) {
        animator.SetTrigger("EdgeGrab");
        if (IsTumbling) {
            statusManager.RemoveStatus(tumblingStatus);
            IsTumbling = false;
        }
        if (IsHelpless) {
            statusManager.RemoveStatus(helplessStatus);
            IsHelpless = false;
        }
        if (movementStatus != null) statusManager.RemoveStatus(movementStatus);
        CanGrabEdge = false;
        isGrabbingEdge = true;
        IgnoreMainJoystick(true);
        grabbingEdge = edge;
        OnLand();
        edgeGrabIntangibilityStatus = new IntangibilityStatus(40); //Grant Intangibility based on percentage
        statusManager.AddStatus(edgeGrabIntangibilityStatus);
        Velocity = Vector2.zero;
        DisableMovement();
        Face(edge.transform.parent.gameObject);
        this.gameObject.transform.position = grabbingEdge.transform.position + this.gameObject.transform.position - edgeGrabTransform.position;
        
        TimerManager.instance.StartTimer(EdgeGrabDurationTimer);

        edge.GetComponent<EdgeBehaviour>().AttachFighter(this.gameObject);
    }

    public virtual void ReleaseEdge() {
        if (grabbingEdge == null) return;
        
        CanGrabEdge = true;
        isGrabbingEdge = false;
        IgnoreMainJoystick(false);

        grabbingEdge.GetComponent<EdgeBehaviour>().ReleaseFighter(this.gameObject);
        grabbingEdge = null;
        EnableMovement();
        this.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
        this.gameObject.transform.localScale = Vector3.one;
        //End intangibility
        statusManager.RemoveStatus(edgeGrabIntangibilityStatus);
        //Edge Grab Immunity after releasing edge
        TimerManager.instance.StartTimer(EdgeGrabImmuneTimer);
        
        //TimerManager.instance.StopTimer(EdgeGrabDurationTimer);
    }

    public virtual void EdgeJump() {
        Velocity = new Vector2(isFacingRight ? 4.0f : -4.0f, 0.0f);
        base.Jump(fullHopHeight);
    }

    //Overrides

    protected override void GroundCheck() {
        base.GroundCheck();
        if (IsGrounded) {
            if (IsTumbling) {
                Debug.Log("Tumble!");
                statusManager.RemoveStatus(tumblingStatus);
                Tumble();
            }
        }
    }

    protected override void OnLand() {
        base.OnLand();
        if (IsTeching) {
            if (mainJoystick.x > 0.5f) { //Tech to the right
                if (IsFacingRight) animator.SetTrigger("TechRollForward");
                else animator.SetTrigger("TechRollBackward");
            }
            else if (mainJoystick.x < -0.5f) { //Tech to the left
                if (IsFacingRight) animator.SetTrigger("TechRollBackward");
                else animator.SetTrigger("TechRollForward");
            }
            else { //Tech in place
                animator.SetTrigger("Tech");
            }
            TimerManager.instance.StopTimer(TechWindowDurationTimer);
            Debug.Log("Teched!");
            IsTeching = false;
        }
        if (IsHelpless) {
            Debug.Log("Land From Helplessness!");
            statusManager.RemoveStatus(helplessStatus);
            IsHelpless = false;
        }
        
    }

    protected override void Jump(float jumpHeight) {
        base.Jump(jumpHeight);
        animator.SetTrigger("Jump");
    }

    protected override void DoubleJump(float jumpHeight) {
        base.DoubleJump(jumpHeight);
        animator.SetTrigger("DoubleJump");
    }

    //Collision with walls

    protected override void OnCollisionEnter2D(Collision2D collision) {
        base.OnCollisionEnter2D(collision);
        if (collision.collider.gameObject.tag == "Wall") {
            //Debug.Log(this.gameObject.name + " collided with wall: " + collision.collider.gameObject.name);
        }
    }

    protected override void OnCollisionStay2D(Collision2D collision) {
        base.OnCollisionStay2D(collision);
        if (collision.collider.gameObject.tag == "Wall") {
            //Debug.Log("Colliding with wall: " + collision.collider.gameObject.name);
            ContactPoint2D[] contacts = new ContactPoint2D[collision.contactCount];
            collision.GetContacts(contacts);
            //Debug Draw Contact point
            foreach (ContactPoint2D contactPoint in contacts) {
                Debug.DrawRay(contactPoint.point, contactPoint.normal, Color.red);
            }
            //Update variables
            collidingWall = collision.collider.gameObject;
            collisionContact = contacts[0].point;
            collisionNormal = contacts[0].normal;
        }
    }

    protected override void OnCollisionExit2D(Collision2D collision) {
        base.OnCollisionExit2D(collision);
        if (collision.collider.gameObject.tag == "Wall") {
            //Debug.Log(this.gameObject.name + " collision with wall over: " + collision.collider.gameObject.name);
            collidingWall = null;
        }
    }
}
