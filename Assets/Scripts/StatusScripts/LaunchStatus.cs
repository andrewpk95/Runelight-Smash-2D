using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchStatus : BaseStatus
{
    public const float LAUNCH_GRAVITY = 20.0f;
    public const float LAUNCH_MAX_FALL_SPEED = 5.0f;
    public const float LAUNCH_MAX_FAST_FALL_SPEED = 8.0f;

    public LaunchStatus() {
        InitializeStatus(); 
    }

    protected override void InitializeStatus() {
        base.InitializeStatus();
        modifiers.Add(new OverrideModifier(Stat.Gravity, LAUNCH_GRAVITY));
        modifiers.Add(new OverrideModifier(Stat.MaxFallSpeed, LAUNCH_MAX_FALL_SPEED));
        modifiers.Add(new OverrideModifier(Stat.MaxFastFallSpeed, LAUNCH_MAX_FAST_FALL_SPEED));
    }
}
