using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpperPanelBahavior : MonoBehaviour
{
    public Text ruleText;

    // Start is called before the first frame update
    void Start()
    {
        ruleText.text = GameStateManager.instance.GameRule.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
