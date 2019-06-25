using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICursor
{
    Player ControllingPlayer {get; set;}
    Token Token {get; set;}
}
