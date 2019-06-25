using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IContinuer
{
    Player ControllingPlayer {get; set;}
	ResultPanelBahavior Panel {get; set;}
}
