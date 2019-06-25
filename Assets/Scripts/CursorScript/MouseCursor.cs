using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCursor : MonoBehaviour, ICursor
{
    private Player controllingPlayer;
    public Player ControllingPlayer {get {return controllingPlayer;} set {controllingPlayer = value;}}

    private Token token;
    public Token Token {get {return token;} set {token = value;}}

    public bool selected;
    public CharacterType selectedCharacter;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateKeyboardInput();
        UpdateMouseInput();
        UpdateTokenPosition();
    }

    void UpdateKeyboardInput() {
        // Start game if all fighters are ready
        if (Input.GetButtonDown("Keyboard_Submit")) {
            Debug.Log("Start fight");
            GameManager.instance.LoadBattleScene();
        }
    }

    void UpdateMouseInput() {
        // Select fighter with left mouse click
        if (Input.GetButtonDown("LeftMouseButton")) {
            CharacterType character = Token.Select(Input.mousePosition);
            if (character != CharacterType.None) {
                selectedCharacter = character;
                selected = true;
                Token.SetPosition(Input.mousePosition);
                EventManager.instance.InvokeOnCharacterSelectEvent(ControllingPlayer, selectedCharacter);
            }
        }
        // Deselect fighter with right mouse click or esc
        if (Input.GetButtonDown("RightMouseButton") || Input.GetButton("Escape")) {
            selectedCharacter = CharacterType.None;
            selected = false;
            Token.Deselect();
            EventManager.instance.InvokeOnCharacterDeSelectEvent(ControllingPlayer);
        }
    }

    void UpdateTokenPosition() {
        if (!selected) {
            Token.SetPosition(Input.mousePosition);
        }
    }
}
