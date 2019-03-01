using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHitbox
{   
    float Damage {get; set;}
    bool HitStun {get; set;}
    int Angle {get; set;}
    float BaseKnockback {get; set;}
    float KnockbackGrowth {get; set;}
    int FreezeFrame {get; set;}
    int ID {get; set;}
    
    void Hit(GameObject target);

    string GetName();

    GameObject GetOwner();

    Vector3 GetWorldPosition();

    void SetOwner(GameObject newOwner);

    List<GameObject> GetCollisionList();
    
    void Reset();
}
