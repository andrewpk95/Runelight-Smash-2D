using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldBreakStatus : BaseStatus
{
    public const float SHIELD_BREAK_GRAVITY = 10.0f;
    public const float SHIELD_BREAK_MAX_FALL_SPEED = 3.0f;
    public const float SHIELD_BREAK_MAX_FAST_FALL_SPEED = 3.0f;

    public ShieldBreakStatus() {
        InitializeStatus(); 
    }

    protected override void InitializeStatus() {
        base.InitializeStatus();
        modifiers.Add(new OverrideModifier(Stat.Gravity, SHIELD_BREAK_GRAVITY));
        modifiers.Add(new OverrideModifier(Stat.MaxFallSpeed, SHIELD_BREAK_MAX_FALL_SPEED));
        modifiers.Add(new OverrideModifier(Stat.MaxFastFallSpeed, SHIELD_BREAK_MAX_FAST_FALL_SPEED));
    }
}
