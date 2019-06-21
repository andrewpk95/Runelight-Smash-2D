using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharacter
{
     bool IsGrounded {get; set;}
     bool IsFacingRight {get; set;}
     Vector2 Velocity {get; set;}
     CharacterStats Stats {get; set;}

     bool IsHelpless {get; set;}
     bool IsTumbling {get; set;}
     bool IsTumbled {get; set;}
     bool IsTeching {get; set;}
     bool CanBeGrabbed {get; set;}
     bool CanGrabEdge {get; set;}

     float GetTargetVelocity(float currentVelocity, float targetVelocity, float accelerationRate);

     void OverrideVelocity(Vector2 velocity);

     void StopOverride();
     
     void Move(Vector2 input);

     void Dash();

     void StopDash();

     void Jump();

     void JumpHold(bool holdingJump);

     void Crouch();

     void UnCrouch();

     void FallThrough();

     void StopFallThrough();

     void FastFall();

     void Flip();

     void Face(GameObject target);

     void EnableAirDeceleration(bool enable);

     void DisableMovement();

     void EnableMovement();

     void Freeze();

     void UnFreeze();

     void IgnoreInput(bool ignore);

     void IgnoreMainJoystick(bool ignore);

     void PreventWalkOffLedge();

     void UnPreventWalkOffLedge();

     void ActionInput(ActionInput actionInput);

     void ShieldHold(bool holdingShield);

     void Helpless();

     void StartTumbling();

     void StopTumbling();

     void Tumble();

     void UnTumble();

     //Grabbing Functions

     void Grab(GameObject target);

     void GetGrabbedBy(GameObject parent);

     void GrabRelease();

     void FreeFromGrab(bool pushed);

     void MashFromGrab();

     void GetMashed(int mashAmount);

     GameObject GetGrabbingFighter();

     GameObject GetGrabber();

     //Edge Functions

     void EdgeGrab(GameObject edge);

     void ReleaseEdge();

     //Death and Respawn Functions

     void OnRespawn();
}