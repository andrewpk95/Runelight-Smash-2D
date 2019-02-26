using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InputManager
{
    //public static float mainHorizontalAxis;
    //public static float mainVerticalAxis;
    //public static bool jumpButtonPressed;
    //public static bool jumpButtonHeld;
    
    public static void readInput() {
        //mainHorizontalAxis = MainHorizontal();
        //mainVerticalAxis = MainVertical();
        //jumpButtonPressed = JumpButtonPressed();
        //jumpButtonHeld = JumpButtonHeld();
    }

    static float MainHorizontal() {
        
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
