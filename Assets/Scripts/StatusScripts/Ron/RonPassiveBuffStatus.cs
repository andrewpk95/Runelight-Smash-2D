using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RonPassiveBuffStatus : SpeedBuffStatus
{
    RonPassive passive;
    
    public RonPassiveBuffStatus(float buffRatio, RonPassive ronPassive) : base(buffRatio) {
        passive = ronPassive;
        IsPermanent = true;
        InitializeStatus();
    }

    public override void OnStatusStay(GameObject entity) {
        foreach (IModifier modifier in modifiers) {
            modifier.ModifyValue = 1.0f + 0.05f * passive.GetStaticCharge();
        }
    }

    public override void OnStatusInterrupt(GameObject entity) {
        //Non-interruptible
    }
}
