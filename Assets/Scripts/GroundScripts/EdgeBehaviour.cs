using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EdgeBehaviour : MonoBehaviour
{
    public List<GameObject> grabbingFighters;
    protected EventManager eventManager;
    
    // Start is called before the first frame update
    void Start()
    {
        grabbingFighters = new List<GameObject>();
        eventManager = (EventManager) GameObject.FindObjectOfType(typeof(EventManager));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collider) {
        GameObject fighter = collider.gameObject.transform.root.gameObject;
        Debug.Log(fighter.name + " collided with " + this.gameObject.name + "!");
        eventManager.InvokeOnEdgeGrabEvent(fighter, this.gameObject);
    }

    public void AttachFighter(GameObject fighter) {
        grabbingFighters.Add(fighter);
        
        fighter.transform.SetParent(this.gameObject.transform);
    }

    public void ReleaseFighter(GameObject fighter) {
        grabbingFighters.Remove(fighter);
        
        fighter.transform.SetParent(null);
    }
}
