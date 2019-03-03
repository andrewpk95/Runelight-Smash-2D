using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchStatus : IStatus
{
    protected bool endStatus;
    public bool EndStatus {get {return endStatus;} set {endStatus = value;}}
    
    public const float LAUNCH_GRAVITY = 20.0f;
    public const float LAUNCH_MAX_FALL_SPEED = 5.0f;
    public const float LAUNCH_MAX_FAST_FALL_SPEED = 8.0f;
    
    protected CharacterStats characterStats;
    protected ICharacter character;
    protected List<IModifier> modifiers;

    public LaunchStatus() {
        InitializeStatus(); 
    }

    protected virtual void InitializeStatus() {
        modifiers = new List<IModifier>();
        modifiers.Add(new OverrideModifier(Stat.Gravity, LAUNCH_GRAVITY));
        modifiers.Add(new OverrideModifier(Stat.MaxFallSpeed, LAUNCH_MAX_FALL_SPEED));
        modifiers.Add(new OverrideModifier(Stat.MaxFastFallSpeed, LAUNCH_MAX_FAST_FALL_SPEED));
    }
    
    public void OnStatusEnter(GameObject entity) {
        character = entity.GetComponent<ICharacter>();
        characterStats = entity.GetComponent<CharacterStats>();
        characterStats.AddModifiers(modifiers);
    }

    public void OnStatusStay(GameObject entity) {
        if (character.IsGrounded()) {
            EndStatus = true;
        }
    }

    public void OnStatusExit(GameObject entity) {
        characterStats.RemoveModifiers(modifiers);
        EndStatus = false;
    }
}
