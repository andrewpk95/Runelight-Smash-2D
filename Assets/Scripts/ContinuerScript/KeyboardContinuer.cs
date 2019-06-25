using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardContinuer : MonoBehaviour, IContinuer
{
    private Player controllingPlayer;
    public Player ControllingPlayer {get {return controllingPlayer;} set {controllingPlayer = value;}}

    [SerializeField] private ResultPanelBahavior panel;
    public ResultPanelBahavior Panel {get {return panel;} set {panel = value;}}

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateKeyboardInput();
    }

    void UpdateKeyboardInput() {
        if (Input.GetButtonDown("Keyboard_Submit")) {
            Panel.NextScreen();
        }
        if (Input.GetButtonDown("Keyboard_Cancel")) {
            Panel.PrevScreen();
        }
    }
}
