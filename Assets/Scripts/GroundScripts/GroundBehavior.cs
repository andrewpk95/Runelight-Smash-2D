using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundBehavior : MonoBehaviour, IPlatform
{
    Collider2D col;
    public Collider2D leftEdge;
    public Collider2D rightEdge;
    
    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<Collider2D>();
        GameObject[] entities = GameObject.FindGameObjectsWithTag("Entity");
        foreach (GameObject entity in entities) {
            IgnoreEdgeCollision(entity.GetComponent<Collider2D>(), true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void IgnorePlatformCollision(Collider2D fighter, bool ignore) {
        //Debug.Log(fighter.gameObject.name + " can land on " + this.gameObject.name + ": " + !ignore);
        Physics2D.IgnoreCollision(col, fighter, ignore);
        //IgnoreEdgeCollision(fighter, ignore);
    }

    public void IgnoreEdgeCollision(Collider2D fighter, bool ignore) {
        Physics2D.IgnoreCollision(leftEdge, fighter, ignore);
        Physics2D.IgnoreCollision(rightEdge, fighter, ignore);
    }

    public void IgnoreAllCollision(Collider2D fighter, bool ignore) {
        IgnorePlatformCollision(fighter, ignore);
        IgnoreEdgeCollision(fighter, ignore);
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
            IgnoreEdgeCollision(fighter, true);
            IgnorePlatformCollision(fighter, false);
        }
    }
}
