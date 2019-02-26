using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharacter
{
     bool IsGrounded();

     bool IsFacingRight();

     Vector2 GetVelocity();

     void SetVelocity(Vector2 velocity);

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

     void EnableAirDeceleration(bool enable);

     void DisableMovement();

     void EnableMovement();

     void IgnoreInput(bool ignore);

     void ActionInput(ActionInput actionInput);
}