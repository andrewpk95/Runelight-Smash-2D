using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardController : MonoBehaviour, IController
{
    private Player controllingPlayer;
    public Player ControllingPlayer {get {return controllingPlayer;} set {controllingPlayer = value;}}

    [SerializeField] private GameObject fighter;
    public GameObject Fighter {get {return fighter;} set {fighter = value;}}
    ICharacter character;

    public Vector2 mainJoystick;
    
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

    //Variables to control shield action input
    bool shieldAction;
    public float dodgeInputThreshold;

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
        character = Fighter.GetComponent<ICharacter>();
        
        //Initialize Input Variables
        mainJoystick = Vector2.zero;
        dashInputTimeLeft = dashInputWindow;
        fallThroughInputTimeLeft = fallThroughInputWindow;
        mouseInputTimeLeft = mouseInputWindow;
        mouseDragVector = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMovementInput();
        UpdateDashInput();
        UpdateDownKeyInput();
        UpdateGrabKeyInput();
        UpdateJumpInput();
        UpdateShieldInput();
        UpdateDirectionInput();
        UpdateMouseInput();
    }
    
    //Move given character
    void UpdateMovementInput() {
        mainJoystick = new Vector2(Input.GetAxis ("Keyboard_MainHorizontal"), Input.GetAxis ("Keyboard_MainVertical"));
        character.Move(mainJoystick);
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
            if (character.IsGrounded) {
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

    void UpdateGrabKeyInput() {
        //Grab key pressed
        if (Input.GetButtonDown ("Keyboard_Grab")) {
            ActionInput actionInput = new ActionInput(InputType.Grab);
            character.ActionInput(actionInput);
        }
    }

    void UpdateShieldInput() {
        //Shield Key pressed
        if (Input.GetButtonDown("Keyboard_Shield")) {
            shieldAction = true;
            if (!character.IsGrounded) {
                ActionInput actionInput = new ActionInput(InputType.Shield);
                character.ActionInput(actionInput);
            }
        }
        if (Input.GetButtonUp("Keyboard_Shield")) {
            shieldAction = false;
        }
        character.ShieldHold(Input.GetButton("Keyboard_Shield"));
    }

    //Apply direction input, such as when the character is shielding or grabbing.
    void UpdateDirectionInput() {
        if (Input.GetButtonDown("Keyboard_MainRight")) {
            ActionInput actionInput = new ActionInput(InputType.Direction, InputDirection.Right, InputStrength.Strong);
            character.ActionInput(actionInput);
        }
        else if (Input.GetButtonDown("Keyboard_MainLeft")) {
            ActionInput actionInput = new ActionInput(InputType.Direction, InputDirection.Left, InputStrength.Strong);
            character.ActionInput(actionInput);
        }
        else if (Input.GetButtonDown("Keyboard_MainDown")) {
            ActionInput actionInput = new ActionInput(InputType.Direction, InputDirection.Down, InputStrength.Strong);
            character.ActionInput(actionInput);
        }
        else if (Input.GetButtonDown("Keyboard_MainUp")) {
            ActionInput actionInput = new ActionInput(InputType.Direction, InputDirection.Up, InputStrength.Strong);
            character.ActionInput(actionInput);
        }
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
            //Debug.Log("Time up");
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
        //actionInput.Print();
    }

    void OnDisable() {
        Cursor.lockState = CursorLockMode.None;
    }
}
