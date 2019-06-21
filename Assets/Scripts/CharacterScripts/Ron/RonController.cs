using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RonController : FighterController, ICharacter
{
    RonPassive passive;
    public float sideSpecialSpeed;
    public bool canUseSideSpecial;

    public float upSpecialHorizontalSpeed;
    public float upSpecialVerticalAcceleration;

    public float downSpecialFallSpeed;
    protected IStatus downSpecialStatus;
    protected Timer MetalFormDurationTimer;
    public const int METAL_FORM_MAXIMUM_DURATION = 120;

    public bool isMetalForm;
    public Color metalFormColor1;
    public Color metalFormColor2;
    public float flashTick;

    // Start is called before the first frame update
    void Awake()
    {
        Initialize();
    }

    protected override void InitializeComponents() {
        base.InitializeComponents();
        passive = GetComponent<RonPassive>();
    }

    protected override void InitializeVariables() {
        base.InitializeVariables();
        MetalFormDurationTimer = new Timer(METAL_FORM_MAXIMUM_DURATION, "Metal Form Duration Timer", OnMetalFormDurationStart, OnMetalFormDurationStop);
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
        base.Tick();
    }

    //Overrides

    protected override void ProcessActionInput() {
        if (isMetalForm) {
            if (actionInputQueue.inputType == InputType.Special) {
                ProcessSpecialInput();
                return;
            }
            else return;
        }
        base.ProcessActionInput();
    }

    protected override void ProcessSpecialInput() {
        if (isMetalForm) {
            animator.SetTrigger("DownSpecialEnd");
            isMetalForm = false;
            TimerManager.instance.StopTimer(MetalFormDurationTimer);
            return;
        }
        base.ProcessSpecialInput();
    }

    protected override void SideSpecial() {
        if (!canUseSideSpecial) return;
        base.SideSpecial();
    }

    public override void Jump() {
        if (isMetalForm) return;
        base.Jump();
    }

    public override void Dash() {
        if (isMetalForm) return;
        base.Dash();
    }

    public override void Crouch() {
        if (isMetalForm) return;
        base.Crouch();
    }

    void OnMetalFormDurationStart() {

    }

    void OnMetalFormDurationStop() {
        animator.SetTrigger("DownSpecialEnd");
        isMetalForm = false;
    }
    
    public void SideSpecialStart() {
        Debug.Log("Side Special Start");
        movementStatus = new RonSideSpecialMovementStatus(sideSpecialSpeed, passive, 1.0f, 16);
        statusManager.AddStatus(movementStatus);
        canUseSideSpecial = false;
    }

    public void UpSpecialStart() {
        Debug.Log("Up Special Start");
        movementStatus = new RonUpSpecialMovementStatus(upSpecialHorizontalSpeed, upSpecialVerticalAcceleration, 1.0f, 6);
        statusManager.AddStatus(movementStatus);
    }

    public void DownSpecialStart() {
        movementStatus = new MovementStatus(Vector2.zero, 50);
        statusManager.AddStatus(movementStatus);
        hurtbox.StartFlashing(metalFormColor1, metalFormColor2, flashTick);
    }

    public void DownSpecialMovement() {
        isMetalForm = true;
        statusManager.RemoveStatus(movementStatus);
        downSpecialStatus = new RonDownSpecialMovementStatus(downSpecialFallSpeed, 1.0f, 120);
        statusManager.AddStatus(downSpecialStatus);
        hurtbox.StopFlashing();
        hurtbox.ChangeSpriteColor(metalFormColor2);

        TimerManager.instance.StartTimer(MetalFormDurationTimer);
    }

    public void DownSpecialEnd() {
        isMetalForm = false;
        statusManager.RemoveStatus(downSpecialStatus);
        movementStatus = new MovementStatus(Vector2.zero, 20);
        statusManager.AddStatus(movementStatus);
        hurtbox.ResetSpriteColor();
    }

    public override void OnHitStun(IAttackHitbox hitbox, GameObject entity) {
        base.OnHitStun(hitbox, entity);
        if (entity.Equals(this.gameObject)) {
            //Allow use of side special again when hitstunned
            canUseSideSpecial = true;
        }
    }

    public override void EdgeGrab(GameObject edge) {
        if (isMetalForm) {
            TimerManager.instance.StopTimer(MetalFormDurationTimer);
            statusManager.RemoveStatus(downSpecialStatus);
            isMetalForm = false;
            hurtbox.ResetSpriteColor();
        }
        base.EdgeGrab(edge);
    }

    public override void OnDeath(GameObject entity) {
        base.OnDeath(entity);
    }

    protected override void GroundCheck() {
        base.GroundCheck();
        if (IsGrounded) canUseSideSpecial = true;
    }

    protected override void OnLand() {
        base.OnLand();
        canUseSideSpecial = true;
    }

    //Reset Override

    protected override void Reset() {
        base.Reset();
        canUseSideSpecial = true;
        TimerManager.instance.StopTimer(MetalFormDurationTimer);
        isMetalForm = false;
    }
}
