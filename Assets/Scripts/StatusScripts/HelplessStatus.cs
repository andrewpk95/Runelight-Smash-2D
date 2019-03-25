using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelplessStatus : BaseStatus
{
    protected float modifyValue;
    public const float AIR_SPEED_DEBUFF_RATIO = 0.7f;
    public const float HELPLESS_GRAVITY = 20.0f;
    public const float HELPLESS_MAX_FALL_SPEED = 5.0f;
    public const float HELPLESS_MAX_FAST_FALL_SPEED = 8.0f;
    
    public HelplessStatus(float debuffRatio) {
        modifyValue = debuffRatio;
        InitializeStatus(); 
    }

    protected override void InitializeStatus() {
        base.InitializeStatus();
        modifiers.Add(new MultiplyModifier(Stat.MaxAirSpeed, AIR_SPEED_DEBUFF_RATIO * modifyValue));
        modifiers.Add(new MultiplyModifier(Stat.AirAccelerationRate, AIR_SPEED_DEBUFF_RATIO * modifyValue));
        modifiers.Add(new MultiplyModifier(Stat.AirDecelerationRate, AIR_SPEED_DEBUFF_RATIO * modifyValue));
        modifiers.Add(new OverrideModifier(Stat.Gravity, HELPLESS_GRAVITY));
        modifiers.Add(new OverrideModifier(Stat.MaxFallSpeed, HELPLESS_MAX_FALL_SPEED));
        modifiers.Add(new OverrideModifier(Stat.MaxFastFallSpeed, HELPLESS_MAX_FAST_FALL_SPEED));
    }

    public override void OnStatusEnter(GameObject entity) {
        base.OnStatusEnter(entity);
        character.CanGrabEdge = true;
    }

    public override void OnStatusExit(GameObject entity) {
        base.OnStatusExit(entity);
        character.CanGrabEdge = false;
    }
}
