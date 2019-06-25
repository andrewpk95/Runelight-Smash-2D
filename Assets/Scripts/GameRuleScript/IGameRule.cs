using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameRule
{
    void StartGame();

    void StopGame();

    List<Player> GetCurrentWinners();

    Dictionary<Player, int> GetRankings();

    string ToString();
}
