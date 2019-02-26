﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardController : MonoBehaviour, IController
{
    public GameObject fighter;
    ICharacter character;
    
    //Variables to control keyboard dash input
    public float dashInputWindow;
    float dashInputTimeLeft;
    bool dashStarted;
    bool maybeAboutToDash;
    static bool isRightDash;

    //Variables to control keyboard fall through input
    public float fallThroughInputWindow;
    float fallThroughInputTimeLeft;
    bool maybeAboutToFallThrough;

    //Variables to control mouse attack input
    public float mouseInputWindow;
    public float mouseMaxDragLength;
    public float mouseWeakDragMinLength;
    public float mouseStrongDragMinLength;
    float mouseInputTimeLeft;
    bool leftMouseClicked;
    bool rightMouseClicked;
    Vector2 mouseDragVector;
    
    // Start is called before the first frame update
    void Start()
    {
        //Initialize character
        character = fighter.GetComponent<ICharacter>();
        
        //Initialize Input Variables
        dashInputTimeLeft = dashInputWindow;
        fallThroughInputTimeLeft = fallThroughInputWindow;
        mouseInputTimeLeft = mouseInputWindow;
        mouseDragVector = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        InputManager.readInput();

        UpdateMovementInput();
        UpdateDashInput();
        UpdateDownKeyInput();
        UpdateJumpInput();

        UpdateMouseInput();
    }
    
    //Move given character
    void UpdateMovementInput() {
        character.Move(new Vector2(Input.GetAxis ("Keyboard_MainHorizontal"), Input.GetAxis ("Keyboard_MainVertical")));
    }

    //Process dash input
    void UpdateDashInput() {
        if (Input.GetButtonDown ("Keyboard_MainHorizontal")) {
            if (maybeAboutToDash) {
                //Same direction pressed twice - dash!
                if ((isRightDash && Input.GetAxis ("Keyboard_MainHorizontal") > 0)
                || (!isRightDash && Input.GetAxis ("Keyboard_MainHorizontal") < 0)) {
                    dashStarted = true;
                    character.Dash();
                    maybeAboutToDash = false;
                }
                //Different direction pressed - update direction
                else {
                    isRightDash = Input.GetAxis ("Keyboard_MainHorizontal") > 0 ? true : false;
                }
            }
            else {
                maybeAboutToDash = true;
                isRightDash = Input.GetAxis ("Keyboard_MainHorizontal") > 0 ? true : false;
            }
        }
        if (Mathf.Abs(Input.GetAxis ("Keyboard_MainHorizontal")) < 0.1f) {
            dashStarted = false;
            character.StopDash();
        }
        if (dashStarted) {
            dashInputTimeLeft = dashInputWindow;
        }
        if (maybeAboutToDash) {
            dashInputTimeLeft -= Time.deltaTime;
        }
        if (dashInputTimeLeft <= 0) {
            maybeAboutToDash = false;
            dashInputTimeLeft = dashInputWindow;
        }
    }

    //Process fall through input
    void UpdateDownKeyInput() {
        if (Input.GetAxis ("Keyboard_MainVertical") < -0.5f) {
            if (character.IsGrounded()) {
                character.Crouch();
                if (Input.GetButtonDown ("Keyboard_MainVertical")) {
                    if (maybeAboutToFallThrough) {
                        character.FallThrough();
                        maybeAboutToFallThrough = false;
                        fallThroughInputTimeLeft = fallThroughInputWindow;
                    }
                    else {
                        maybeAboutToFallThrough = true;
                    }
                }
            }
            else {
                if (Input.GetButtonDown ("Keyboard_MainVertical")) {
                    character.FastFall();
                }
                character.FallThrough();
            }
            
        }
        else {
            character.UnCrouch();
            character.StopFallThrough();
        }
        if (maybeAboutToFallThrough) {
            fallThroughInputTimeLeft -= Time.deltaTime;
        }
        if (fallThroughInputTimeLeft <= 0) {
            maybeAboutToFallThrough = false;
            fallThroughInputTimeLeft = fallThroughInputWindow;
        }
        
    }

    void UpdateJumpInput() {
        //Jump Key pressed -> Jump
        if (Input.GetButtonDown ("Keyboard_Jump")) {
            character.Jump();
        }
        character.JumpHold(Input.GetButton("Keyboard_Jump"));
    }

    void UpdateMouseInput() {
        //Esc to unlock cursor
        if (Input.GetButton("Escape")) {
            Cursor.lockState = CursorLockMode.None;
        }
        //Mouse click to begin attack input
        if (Input.GetButtonDown("LeftMouseButton") && !rightMouseClicked) {
            Cursor.lockState = CursorLockMode.Locked;
            leftMouseClicked = true;
        }
        if (Input.GetButtonDown("RightMouseButton") && !leftMouseClicked) {
            Cursor.lockState = CursorLockMode.Locked;
            rightMouseClicked = true;
        }
        //Add mouse movement to mouse drag length during input time
        if (leftMouseClicked || rightMouseClicked) {
            mouseDragVector += new Vector2(Input.GetAxis("MouseMovementX"), Input.GetAxis("MouseMovementY"));
            mouseInputTimeLeft -= Time.deltaTime;
        }
        //End mouse input when mouse is released
        if (leftMouseClicked && Input.GetButtonUp("LeftMouseButton")) {
            //Debug.Log("Left mouse: " + mouseDragVector);
            ProcessMouseInput();
            ResetMouseInput();
        }
        if (rightMouseClicked && Input.GetButtonUp("RightMouseButton")) {
            //Debug.Log("Right mouse: " + mouseDragVector);
            ProcessMouseInput();
            ResetMouseInput();
        }
        //Stop mouse input when input time is up
        if (mouseInputTimeLeft < 0) {
            Debug.Log("Time up");
            ResetMouseInput();
        }
    }

    void ResetMouseInput() {
        leftMouseClicked = rightMouseClicked = false;
        mouseDragVector = Vector2.zero;
        mouseInputTimeLeft = mouseInputWindow;
    }

    void ProcessMouseInput() {
        //Determine input strength using parameters above
        float dragStrength = Mathf.Clamp(mouseDragVector.magnitude, 0.0f, mouseMaxDragLength);
        InputStrength inputStrength;
        if (dragStrength >= mouseStrongDragMinLength) {
            inputStrength = InputStrength.Strong;
        }
        else if (dragStrength >= mouseWeakDragMinLength) {
            inputStrength = InputStrength.Weak;
        }
        else {
            inputStrength = InputStrength.None;
        }
        float angle = Vector2.SignedAngle(Vector2.right, mouseDragVector);
        //Make ActionInput and pass it to the controlling character
        ActionInput actionInput;
        if (leftMouseClicked) {
            actionInput = new ActionInput(InputType.Attack, angle, inputStrength);
            character.ActionInput(actionInput);
        }
        else if (rightMouseClicked) {
            actionInput = new ActionInput(InputType.Special, angle, inputStrength);
            character.ActionInput(actionInput);
        }
        else {
            return;
        }
        //Debug
        actionInput.Print();
    }
}
