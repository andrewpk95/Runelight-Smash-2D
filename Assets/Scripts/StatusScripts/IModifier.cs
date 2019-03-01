using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IModifier
{
    Stat TargetStat {get; set;}
    float ModifyValue {get; set;}

    int Priority {get; set;} //The closer this number is to 0, the more priority it has
    
    float Apply(float stat);
}
