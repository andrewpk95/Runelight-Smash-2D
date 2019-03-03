using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldBreakStatus : IStatus
{
    protected bool endStatus;
    public bool EndStatus {get {return endStatus;} set {endStatus = value;}}
    
    public const float SHIELD_BREAK_GRAVITY = 10.0f;
    public const float SHIELD_BREAK_MAX_FALL_SPEED = 3.0f;
    public const float SHIELD_BREAK_MAX_FAST_FALL_SPEED = 3.0f;
    
    protected CharacterStats characterStats;
    protected ICharacter character;
    protected List<IModifier> modifiers;

    public ShieldBreakStatus() {
        InitializeStatus(); 
    }

    protected virtual void InitializeStatus() {
        modifiers = new List<IModifier>();
        modifiers.Add(new OverrideModifier(Stat.Gravity, SHIELD_BREAK_GRAVITY));
        modifiers.Add(new OverrideModifier(Stat.MaxFallSpeed, SHIELD_BREAK_MAX_FALL_SPEED));
        modifiers.Add(new OverrideModifier(Stat.MaxFastFallSpeed, SHIELD_BREAK_MAX_FAST_FALL_SPEED));
    }
    
    public void OnStatusEnter(GameObject entity) {
        character = entity.GetComponent<ICharacter>();
        characterStats = entity.GetComponent<CharacterStats>();
        characterStats.AddModifiers(modifiers);
    }

    public void OnStatusStay(GameObject entity) {
        
    }

    public void OnStatusExit(GameObject entity) {
        characterStats.RemoveModifiers(modifiers);
        EndStatus = false;
    }
}
