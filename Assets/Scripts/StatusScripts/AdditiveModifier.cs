using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdditiveModifier : IModifier
{
    protected Stat targetStat;
    protected float modifyValue;
    protected int priority = 2;
    
    public Stat TargetStat {get {return targetStat;} set {targetStat = value;}}
    public float ModifyValue {get {return modifyValue;} set {modifyValue = value;}}

    public int Priority {get {return priority;} set {priority = value;}}

    public AdditiveModifier(Stat stat, float val) {
        TargetStat = stat;
        ModifyValue = val;
    }

    public float Apply(float stat) {
        return stat + ModifyValue;
    }
}
