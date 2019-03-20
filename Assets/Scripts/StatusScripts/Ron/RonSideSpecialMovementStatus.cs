using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RonSideSpecialMovementStatus : MovementStatus
{
    protected float ratio;
    protected float sideSpecialSpeed;
    protected float sideSpecialDecelerationRate;
    
    protected const int START_DURATION = 16;
    protected const int MOVEMENT_DURATION = 24;
    protected const int END_DURATION = 20;

    RonPassive passive;

    public RonSideSpecialMovementStatus(float speed, RonPassive ronPassive, float movementRatio, int duration) : base(duration) {
        passive = ronPassive;
        sideSpecialSpeed = speed;
        sideSpecialDecelerationRate = 30.0f;
        ratio = movementRatio;
        frameDuration = START_DURATION + MOVEMENT_DURATION + END_DURATION;
        InitializeStatus();
    }

    public override void OnStatusStay(GameObject entity) {
        //Start of the up special
        if (durationLeft < START_DURATION) {
            targetVelocity = Vector2.zero;
        }
        //Movement of the up special
        else if (durationLeft < START_DURATION + MOVEMENT_DURATION) {
            float speedMultiplier = 1.0f + 0.5f * passive.GetStaticCharge() / passive.maxStaticCharge;
            targetVelocity = new Vector2(character.IsFacingRight ? sideSpecialSpeed * speedMultiplier * ratio : -sideSpecialSpeed * speedMultiplier * ratio, 0);
        }
        //End of the up special
        else if (durationLeft < frameDuration) {
            targetVelocity = new Vector2(character.GetTargetVelocity(character.Velocity.x, 0.0f, sideSpecialDecelerationRate),
                                            character.GetTargetVelocity(character.Velocity.y, 0.0f, sideSpecialDecelerationRate));
        }
        base.OnStatusStay(entity);
    }

    public override void OnStatusExit(GameObject entity) {
        base.OnStatusExit(entity);
        Debug.Log("Side Special Done");
    }
}
