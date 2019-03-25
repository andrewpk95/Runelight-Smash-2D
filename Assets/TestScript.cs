using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    public GameObject test;
    //public LayerMask mask;

    public Coroutine timer;
    
    // Start is called before the first frame update
    void Start()
    {
        //timer = Timer.instance.CreateTimer(120, Empty, Empty, "Test Timer");
    }

    void Empty() {
        Debug.Log("Haha");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //if (Time.frameCount > 50) Timer.instance.StopTimer(timer);
    }
}
