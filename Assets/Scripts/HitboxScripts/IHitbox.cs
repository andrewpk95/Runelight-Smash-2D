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

    List<GameObject> GetCollisionList();
    
    void Reset();
}
