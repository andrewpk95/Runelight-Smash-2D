using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InputManager
{
    static float KeyboardMainHorizontal() {
        
        return Input.GetAxis ("Keyboard_MainHorizontal");
    }

    static float MainVertical() {
        
        return Input.GetAxis ("Keyboard_MainVertical");
    }

    static bool JumpButtonPressed() {
        return Input.GetButtonDown("Keyboard_Jump");
    }

    static bool JumpButtonHeld() {
        return Input.GetButton("Keyboard_Jump");
    }
}
