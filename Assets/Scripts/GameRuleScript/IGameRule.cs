using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameRule
{
    List<GameObject> GetCurrentWinners();
}
