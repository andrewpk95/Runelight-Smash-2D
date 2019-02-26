using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHitbox
{   
    void Hit(GameObject target);

    int GetID();

    void SetHitStun(bool hit);

    string GetName();

    GameObject GetOwner();

    void SetOwner(GameObject newOwner);

    float GetDamage();

    void SetDamage(float newDamage);

    List<GameObject> GetCollisionList();
    
    void Reset();
}
