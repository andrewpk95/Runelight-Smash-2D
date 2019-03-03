using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBuffStatus : IStatus
{
    protected bool endStatus;
    public bool EndStatus {get {return endStatus;} set {endStatus = value;}}
    
    protected CharacterStats character;
    protected List<IModifier> modifiers;
    protected float modifyValue;

    public SpeedBuffStatus(float buffRatio) {
        modifyValue = buffRatio;
        InitializeStatus(); 
    }

    protected virtual void InitializeStatus() {
        modifiers = new List<IModifier>();
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
    
    public virtual void OnStatusEnter(GameObject entity) {
        character = entity.GetComponent<CharacterStats>();
        character.AddModifiers(modifiers);
    }

    public virtual void OnStatusStay(GameObject entity) {

    }

    public virtual void OnStatusExit(GameObject entity) {
        character.RemoveModifiers(modifiers);
    }
}