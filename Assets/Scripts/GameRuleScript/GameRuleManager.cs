using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRuleManager : MonoBehaviour
{
    IGameRule gameRule;
    int interval;
    
    // Start is called before the first frame update
    void Start()
    {
        gameRule = new TimeGameRule(5);
        interval = 120;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        interval--;
        if (interval < 0) {
            string log = "Current Winners: ";
            foreach (GameObject fighter in gameRule.GetCurrentWinners()) {
                log = log + fighter.name + " ";
            }
            Debug.Log(log);
            interval = 120;
        }
    }
}
