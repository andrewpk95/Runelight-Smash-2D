using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour, IController
{
    public GameObject fighter;
    ICharacter character;
    
    ActionInput actionInput;
    
    // Start is called before the first frame update
    void Start()
    {
        //Initialize character
        character = fighter.GetComponent<ICharacter>();
        actionInput = new ActionInput(InputType.Attack, InputDirection.Up, InputStrength.Strong);
    }

    // Update is called once per frame
    void Update()
    {
        
        character.ActionInput(actionInput);
    }
}
