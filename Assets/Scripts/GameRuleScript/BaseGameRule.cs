using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BaseGameRule : IGameRule
{
    protected GameObject[] spawnPointList;

    UnityAction<GameObject> listener;

    public BaseGameRule() {
        Initialize();
    }

    protected virtual void Initialize() {

    }

    public virtual void StartGame() {
        spawnPointList = GameObject.FindGameObjectsWithTag("Respawn");
        listener = new UnityAction<GameObject>(OnEntityDeath);
        EventManager.instance.StartListeningToOnDeathEvent(listener);
    }

    public virtual void StopGame() {
        EventManager.instance.StopListeningToOnDeathEvent(listener);
    }

    public override string ToString() {
        return "BaseGameRule";
    }

    protected virtual void OnEntityDeath(GameObject entity) {
        foreach (Player player in Player.GetPlayers()) {
            if (player != null) {
                if (entity.Equals(player.ControllingCharacter)) {
                    player.Deaths++;
                    break;
                }
            }
        }
        GameObject killer = entity.GetComponent<IDamageable>().LastDamagedBy;
        if (killer == null) return;
        foreach (Player player in Player.GetPlayers()) {
            if (player != null) {
                if (killer.Equals(player.ControllingCharacter)) {
                    player.Kills++;
                    entity.GetComponent<IDamageable>().LastDamagedBy = null;
                    break;
                }
            }
        }
    }

    protected virtual void Respawn(GameObject entity) {
        if (spawnPointList.Length <= 0) {
            Debug.Log("No available spawn points!");
        }
        else {
            int index = Random.Range(0, spawnPointList.Length);
            entity.transform.position = spawnPointList[index].transform.position;
            entity.GetComponent<ICharacter>().OnRespawn();
        }
    }

    public virtual List<Player> GetCurrentWinners() {
        List<Player> winners = new List<Player>();
        int maxScore = -999;
        foreach (Player player in Player.GetPlayers()) {
            if (player != null) {
                int score = player.Total;
                if (score > maxScore) {
                    winners.Clear();
                    winners.Add(player);
                    maxScore = score;
                }
                else if (score == maxScore) {
                    winners.Add(player);
                }
            }
        }
        return winners;
    }

    public virtual Dictionary<Player, int> GetRankings() {
        Dictionary<Player, int> rankings = new Dictionary<Player, int>();
        List<Player> players = new List<Player>();
        foreach (Player player in Player.GetPlayers()) {
            if (player != null) {
                players.Add(player);
            }
        }
        players.Sort(delegate(Player x, Player y) { return y.Total - x.Total;});
        int rank = 0;
        int maxScore = 999;
        foreach (Player player in players) {
            if (player != null) {
                if (player.Total < maxScore) {
                    rank++;
                    maxScore = player.Total;
                    rankings.Add(player, rank);
                }
                else if (player.Total == maxScore) {
                    rankings.Add(player, rank);
                }
            }
        }
        return rankings;
    }
}
