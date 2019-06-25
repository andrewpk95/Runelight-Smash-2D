using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour, IController
{
    private Player controllingPlayer;
    public Player ControllingPlayer {get {return controllingPlayer;} set {controllingPlayer = value;}}

    [SerializeField] private GameObject fighter;
    public GameObject Fighter {get {return fighter;} set {fighter = value;}}
    
    ICharacter character;
    
    ActionInput actionInput;
    
    // Start is called before the first frame update
    void Start()
    {
        //Initialize character
        character = Fighter.GetComponent<ICharacter>();
        //actionInput = new ActionInput(InputType.Grab);
        actionInput = new ActionInput(InputType.Attack, InputDirection.Right, InputStrength.Strong);
    }

    // Update is called once per frame
    void Update()
    {
        character.ActionInput(actionInput);
        
    }
}
