using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseStatus : IStatus
{
    protected bool endStatus;
    public bool EndStatus {get {return endStatus;} set {endStatus = value;}}
    protected bool isPermanent;
    public bool IsPermanent {get {return isPermanent;} set {isPermanent = value;}}
    
    protected CharacterStats characterStats;
    protected ICharacter character;
    protected StatusManager statusManager;
    protected List<IModifier> modifiers;

    public BaseStatus() {
        InitializeStatus(); 
    }

    protected virtual void InitializeStatus() {
        modifiers = new List<IModifier>();
    }
    
    public virtual void OnStatusEnter(GameObject entity) {
        character = entity.GetComponent<ICharacter>();
        statusManager = entity.GetComponent<StatusManager>();
        characterStats = character.Stats;
        characterStats.AddModifiers(modifiers);
    }

    public virtual void OnStatusStay(GameObject entity) {
        
    }

    public virtual void OnStatusExit(GameObject entity) {
        characterStats.RemoveModifiers(modifiers);
        EndStatus = false;
    }

    public virtual void OnStatusInterrupt(GameObject entity) {
        OnStatusExit(entity);
    }
}
