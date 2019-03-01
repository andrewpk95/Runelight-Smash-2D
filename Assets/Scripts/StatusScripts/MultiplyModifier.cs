using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplyModifier : IModifier
{
    protected Stat targetStat;
    protected float modifyValue;
    protected int priority = 1;
    
    public Stat TargetStat {get {return targetStat;} set {targetStat = value;}}
    public float ModifyValue {get {return modifyValue;} set {modifyValue = value;}}

    public int Priority {get {return priority;} set {priority = value;}}

    public MultiplyModifier(Stat stat, float val) {
        TargetStat = stat;
        ModifyValue = val;
    }

    public float Apply(float stat) {
        return stat * ModifyValue;
    }
}
