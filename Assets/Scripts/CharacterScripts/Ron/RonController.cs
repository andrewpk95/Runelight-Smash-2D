using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RonController : FighterController, ICharacter
{
    RonPassive passive;
    public float sideSpecialSpeed;
    public bool isSideSpecial;

    public float upSpecialHorizontalSpeed;
    public float upSpecialVerticalAcceleration;
    public bool isUpSpecial;
    public bool isUpSpecialStop;

    public float downSpecialFallSpeed;
    public bool isDownSpecial;

    public bool isMetalForm;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    protected override void InitializeComponents() {
        base.InitializeComponents();
        passive = GetComponent<RonPassive>();
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
        if (isSideSpecial) {
            float speedMultiplier = 1.0f + 0.5f * passive.GetStaticCharge() / passive.maxStaticCharge;
            overridingVelocity = new Vector2(isFacingRight ? sideSpecialSpeed * speedMultiplier : -sideSpecialSpeed * speedMultiplier, 0);
        }
        if (isUpSpecial) {
            overridingVelocity = new Vector2(isFacingRight ? upSpecialHorizontalSpeed : -upSpecialHorizontalSpeed, 
                                                GetTargetVelocity(velocity.y, 50.0f, upSpecialVerticalAcceleration));
        }
        if (isUpSpecialStop) {
            overridingVelocity = new Vector2(GetTargetVelocity(velocity.x, 0.0f, 50.0f), 
                                                GetTargetVelocity(velocity.y, 0.0f, 50.0f));
        }
        if (isDownSpecial) {
            overridingVelocity = new Vector2(0, -downSpecialFallSpeed);
        }

        if (movementEnabled) {
            if (overrideVelocity) {
                velocity = overridingVelocity;
                rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
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

    public void InterruptMovement() {
        isSideSpecial = false;
        isUpSpecial = false;
        isUpSpecialStop = false;
        isDownSpecial = false;
        StopOverride();
    }
    
    public void SideSpecialStart() {
        OverrideVelocity(Vector2.zero);
    }

    public void SideSpecialMovement() {
        isSideSpecial = true;
        float speedMultiplier = 1.0f + 0.5f * passive.GetStaticCharge() / passive.maxStaticCharge;
        Vector2 direction = new Vector2(isFacingRight? sideSpecialSpeed * speedMultiplier : -sideSpecialSpeed * speedMultiplier, 0);
        OverrideVelocity(direction);
    }

    public void SideSpecialEnd() {
        isSideSpecial = false;
        StopOverride();
    }

    public void UpSpecialStart() {
        OverrideVelocity(Vector2.zero);
    }

    public void UpSpecialMovement() {
        isUpSpecial = true;
    }

    public void UpSpecialStopping() {
        isUpSpecial = false;
        isUpSpecialStop = true;
    }

    public void UpSpecialEnd() {
        isUpSpecial = false;
        isUpSpecialStop = false;
        StopOverride();
    }

    public void DownSpecialStart() {
        OverrideVelocity(Vector2.zero);
    }

    public void DownSpecialMovement() {
        isDownSpecial = true;
        overridingVelocity = new Vector2(0, -downSpecialFallSpeed);
    }

    public void DownSpecialEnd() {
        isDownSpecial = false;
        OverrideVelocity(Vector2.zero);
        StopOverride();
    }

}
