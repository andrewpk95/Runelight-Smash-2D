using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IController
{
    Player ControllingPlayer {get; set;}
	GameObject Fighter {get; set;}
}
