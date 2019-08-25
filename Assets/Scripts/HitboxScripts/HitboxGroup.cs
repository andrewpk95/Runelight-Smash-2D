using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxGroup : MonoBehaviour
{
    public List<GameObject> Victims;
    public IHitbox[] Hitboxes;

    // Start is called before the first frame update
    void Start()
    {
        Victims = new List<GameObject>();
        Hitboxes = GetComponentsInChildren<IHitbox>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
