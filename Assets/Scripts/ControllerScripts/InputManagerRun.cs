using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManagerRun : MonoBehaviour
{
    public static InputManagerRun instance;

    // Start is called before the first frame update
    void Start()
    {
        //Singleton Pattern
        if (instance == null) instance = this;
        else if (instance != this) Destroy(this.gameObject);    
        
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
