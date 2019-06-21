using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BaseGameRule : IGameRule
{
    protected GameObject[] fighterList;
    protected GameObject[] spawnPointList;

    protected Dictionary<GameObject, int> kills;
    protected Dictionary<GameObject, int> deaths;

    public BaseGameRule() {
        Initialize();
    }

    protected virtual void Initialize() {
        fighterList = GameObject.FindGameObjectsWithTag("Entity");
        spawnPointList = GameObject.FindGameObjectsWithTag("Respawn");
        kills = new Dictionary<GameObject, int>();
        foreach (GameObject fighter in fighterList) {
            kills.Add(fighter, 0);
        }
        deaths = new Dictionary<GameObject, int>();
        foreach (GameObject fighter in fighterList) {
            deaths.Add(fighter, 0);
        }
        EventManager.instance.StartListeningToOnDeathEvent(new UnityAction<GameObject>(OnEntityDeath));
    }

    protected virtual void OnEntityDeath(GameObject entity) {
        deaths[entity]++;
        GameObject killer = entity.GetComponent<IDamageable>().LastDamagedBy;
        if (killer != null) {
            kills[killer]++;
        }
    }

    protected virtual void Respawn(GameObject entity) {
        if (spawnPointList.Length <= 0) {
            Debug.Log("No available spawn points!");
        }
        else {
            entity.transform.position = spawnPointList[0].transform.position;
            entity.GetComponent<ICharacter>().OnRespawn();
        }
    }

    public virtual List<GameObject> GetCurrentWinners() {
        //Get Max kills
        List<GameObject> winners = new List<GameObject>();
        int maxScore = -999;
        foreach (GameObject fighter in fighterList) {
            int score = kills[fighter] - deaths[fighter];
            if (score > maxScore) {
                winners.Clear();
                winners.Add(fighter);
                maxScore = score;
            }
            else if (score == maxScore) {
                winners.Add(fighter);
            }
        }
        return winners;
    }
}
