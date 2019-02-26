using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    public Collider2D hitbox;
    public LayerMask mask;
    
    // Start is called before the first frame update
    void Start()
    {
        hitbox = GetComponent<Collider2D>();
        mask = Physics2D.GetLayerCollisionMask (gameObject.layer);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ContactFilter2D contactFilter = new ContactFilter2D();
        contactFilter.SetLayerMask(mask);
        contactFilter.useLayerMask = true;
        Collider2D[] result = new Collider2D[500];
        int numResults = Physics2D.OverlapCollider(hitbox, contactFilter, result);
        //Debug.Log(numResults);
        
        for(int i = 0; i < numResults; i++) {
            GameObject hit = result[i].gameObject.transform.root.gameObject;
            Debug.Log(hit.gameObject.name);
            
        }
    }
}
