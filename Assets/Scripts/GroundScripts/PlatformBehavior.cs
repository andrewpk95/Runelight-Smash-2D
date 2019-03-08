using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformBehavior : MonoBehaviour, IGround
{
    Collider2D col;
    
    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<Collider2D>();
        GameObject[] entities = GameObject.FindGameObjectsWithTag("Entity");
        foreach (GameObject entity in entities) {
            IgnoreCollision(entity.GetComponent<Collider2D>(), true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void IgnoreCollision(Collider2D fighter, bool ignore) {
        //Debug.Log(fighter.gameObject.name + " can land on " + this.gameObject.name + ": " + !ignore);
        Physics2D.IgnoreCollision(col, fighter, ignore);
        //IgnoreEdgeCollision(fighter, ignore);
    }

    void OnCollisionEnter2D(Collision2D collided) {
        if (collided.gameObject.tag == "Entity") {
            //Debug.Log(collided.gameObject.name + " landed on " + this.gameObject.name);
            Collider2D fighter = collided.collider;
        }
    }

    void OnCollisionExit2D(Collision2D collided) {
        if (collided.gameObject.tag == "Entity") {
            //Debug.Log(collided.gameObject.name + " left " + this.gameObject.name);
            Collider2D fighter = collided.collider;
            
            //IgnoreCollision(fighter, false);
        }
    }
}
