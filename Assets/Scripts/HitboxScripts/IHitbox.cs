using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHitbox
{   
    int ID {get; set;}
    GameObject GameObject {get;}
    
    void OnHit(GameObject target);

    void OnClash(int clashFrame);

    string GetName();

    GameObject GetOwner();

    Vector3 GetWorldPosition();

    void SetOwner(GameObject newOwner);

    List<GameObject> GetCollisionList();

    List<GameObject> GetVictimList();

    void AddToVictimList(GameObject victim);

    void Enable();
    
    void Reset();
}
