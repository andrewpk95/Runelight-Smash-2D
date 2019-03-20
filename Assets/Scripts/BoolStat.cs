using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoolStat
{
    public bool stat;
    public int setCount;
    bool defaultStat;

    public BoolStat(bool defaultValue) {
        defaultStat = defaultValue;
        stat = defaultStat;
    }

    public void Switch(bool switchValue) {
        if (switchValue ^ defaultStat) SwitchOn();
        else SwitchOff();
    }

    public void SwitchOn() {
        setCount++;
        stat = setCount > 0 ^ defaultStat;
    }

    public void SwitchOff() {
        setCount--;
        stat = setCount > 0 ^ defaultStat;
    }
}
