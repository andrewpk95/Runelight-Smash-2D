using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBuffStatus : BaseStatus
{
    protected float modifyValue;

    public SpeedBuffStatus(float buffRatio) {
        modifyValue = buffRatio;
        InitializeStatus(); 
    }

    protected override void InitializeStatus() {
        base.InitializeStatus();
        modifiers.Add(new MultiplyModifier(Stat.MaxWalkSpeed, modifyValue));
        modifiers.Add(new MultiplyModifier(Stat.WalkAccelerationRate, modifyValue));
        modifiers.Add(new MultiplyModifier(Stat.GroundDecelerationRate, modifyValue));
        modifiers.Add(new MultiplyModifier(Stat.InitialDashSpeed, modifyValue));
        modifiers.Add(new MultiplyModifier(Stat.MaxDashSpeed, modifyValue));
        modifiers.Add(new MultiplyModifier(Stat.DashAccelerationRate, modifyValue));
        modifiers.Add(new MultiplyModifier(Stat.MaxAirSpeed, modifyValue));
        modifiers.Add(new MultiplyModifier(Stat.AirAccelerationRate, modifyValue));
        modifiers.Add(new MultiplyModifier(Stat.AirDecelerationRate, modifyValue));
        modifiers.Add(new MultiplyModifier(Stat.Gravity, modifyValue));
        modifiers.Add(new MultiplyModifier(Stat.MaxFallSpeed, modifyValue));
        modifiers.Add(new MultiplyModifier(Stat.MaxFastFallSpeed, modifyValue));
    }
}