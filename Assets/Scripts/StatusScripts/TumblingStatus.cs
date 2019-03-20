﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TumblingStatus : BaseStatus
{
    public const float TUMBLE_GRAVITY = 20.0f;
    public const float TUMBLE_MAX_FALL_SPEED = 5.0f;
    public const float TUMBLE_MAX_FAST_FALL_SPEED = 8.0f;

    public const int MINIMUM_STATUS_DURATION = 25;
    protected int durationLeft;

    public TumblingStatus() {
        InitializeStatus(); 
    }

    protected override void InitializeStatus() {
        base.InitializeStatus();
        modifiers.Add(new OverrideModifier(Stat.Gravity, TUMBLE_GRAVITY));
        modifiers.Add(new OverrideModifier(Stat.MaxFallSpeed, TUMBLE_MAX_FALL_SPEED));
        modifiers.Add(new OverrideModifier(Stat.MaxFastFallSpeed, TUMBLE_MAX_FAST_FALL_SPEED));
    }

    public override void OnStatusEnter(GameObject entity) {
        base.OnStatusEnter(entity);
        durationLeft = 0;
    }

    public override void OnStatusStay(GameObject entity) {
        base.OnStatusStay(entity);
        durationLeft++;
        
    }

    public override void OnStatusExit(GameObject entity) {
        base.OnStatusExit(entity);
        durationLeft = 0;
    }
}
