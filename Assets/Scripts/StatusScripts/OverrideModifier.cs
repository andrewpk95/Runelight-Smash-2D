using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverrideModifier : IModifier
{
    protected Stat targetStat;
    protected float modifyValue;
    protected int priority = 10;
    
    public Stat TargetStat {get {return targetStat;} set {targetStat = value;}}
    public float ModifyValue {get {return modifyValue;} set {modifyValue = value;}}

    public int Priority {get {return priority;} set {priority = value;}}

    public OverrideModifier(Stat stat, float val) {
        TargetStat = stat;
        ModifyValue = val;
    }

    public float Apply(float stat) {
        return ModifyValue;
    }
}
