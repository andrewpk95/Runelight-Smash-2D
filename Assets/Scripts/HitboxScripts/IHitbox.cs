using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHitbox
{   
    int ID {get; set;}
    
    void OnHit(GameObject target);

    string GetName();

    GameObject GetOwner();

    Vector3 GetWorldPosition();

    void SetOwner(GameObject newOwner);

    List<GameObject> GetCollisionList();
    
    void Reset();
}
