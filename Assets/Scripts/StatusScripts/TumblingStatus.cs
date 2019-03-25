using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TumblingStatus : BaseStatus
{
    public const float TUMBLE_GRAVITY = 20.0f;
    public const float TUMBLE_MAX_FALL_SPEED = 5.0f;
    public const float TUMBLE_MAX_FAST_FALL_SPEED = 8.0f;

    public TumblingStatus() {
        InitializeStatus(); 
    }

    protected override void InitializeStatus() {
        base.InitializeStatus();
        modifiers.Add(new OverrideModifier(Stat.Gravity, TUMBLE_GRAVITY));
        modifiers.Add(new OverrideModifier(Stat.MaxFallSpeed, TUMBLE_MAX_FALL_SPEED));
        modifiers.Add(new OverrideModifier(Stat.MaxFastFallSpeed, TUMBLE_MAX_FAST_FALL_SPEED));
    }
}
