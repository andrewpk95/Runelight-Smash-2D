using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InputType
{
    Attack, Special, Shield, Grab, None
}

public enum InputDirection
{
    Right, Up, Left, Down, None
}

public enum InputStrength
{
    None, Weak, Strong
}

public class ActionInput
{
    public InputType inputType;
    public InputDirection inputDirection;
    public float inputAngle;
    public InputStrength inputStrength;

    public ActionInput (InputType t, float a, InputStrength s) {
        inputType = t;
        if (s == InputStrength.None) {
            inputDirection = InputDirection.None;
            inputAngle = 0.0f;
        }
        else {
            inputDirection = AngleToDirection(a);
            inputAngle = a;
        }
        inputStrength = s;
    }
    
    public ActionInput (InputType t, InputDirection d, InputStrength s) {
        inputType = t;
        inputDirection = d;
        inputAngle = DirectionToAngle(d);
        if (d == InputDirection.None) {
            inputStrength = InputStrength.None;
        }
        else {
            inputStrength = s;
        }
    }

    public ActionInput (InputType t) {
        inputType = t;
        inputDirection = InputDirection.None;
        inputAngle = 0.0f;
        inputStrength = InputStrength.None;
    }

    public static float DirectionToAngle(InputDirection dir) {
        if (dir == InputDirection.Right) return 0.0f;
        if (dir == InputDirection.Up) return 90.0f;
        if (dir == InputDirection.Left) return 180.0f;
        if (dir == InputDirection.Down) return -90.0f;
        return 0.0f;
    }

    public static InputDirection AngleToDirection(float angle) {
        if (angle >= -135 && angle < -45) return InputDirection.Down;
        if (angle >= -45 && angle < 45) return InputDirection.Right;
        if (angle >= 45 && angle < 135) return InputDirection.Up;
        return InputDirection.Left;
    }

    public void Print() {
        Debug.Log("InputType: " + inputType + ", InputDirection: " + inputDirection + ", InputAngle: " + inputAngle + ", InputStrength: " + inputStrength);
    }
}