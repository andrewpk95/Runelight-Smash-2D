using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameRule
{
    void StartGame();

    void StopGame();

    void OnEntityDeath(GameObject entity);

    List<Player> GetCurrentWinners();

    Dictionary<Player, int> GetRankings();

    string ToString();
}
