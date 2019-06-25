using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public const int PLAYER_LIMIT = 4;

    static Player[] Players = new Player[PLAYER_LIMIT];
    
    public string playerName;
    public int playerNumber;

    public CharacterType selectedCharacter;
    public bool isReady;

    public GameObject ControllingCharacter;
    public int Kills;
    public int Deaths;
    public int Total {get {return Kills - Deaths;}}

    public bool isComputer;

    public Player() {
        //Insert to player list
        for (int i = 0; i < Players.Length; i++) {
            if (Players[i] == null) {
                Players[i] = this;
                playerNumber = (i + 1);
                playerName = "Player " + playerNumber;
                Debug.Log(playerName + " Created!");
                return;
            }
        }
        Debug.Log("No more space to create player");
    }

    public Player(bool isAI) {
        isComputer = isAI;
        for (int i = 0; i < Players.Length; i++) {
            if (Players[i] == null) {
                Players[i] = this;
                playerNumber = (i + 1);
                playerName = "CPU " + playerNumber;
                Debug.Log(playerName + " Created!");
                return;
            }
        }
        Debug.Log("No more space to create player");
    }

    public static Player FindPlayer(int playerNumber) {
        return Players[playerNumber - 1];
    }

    public static void RemovePlayer(Player player) {
        for (int i = 0; i < Players.Length; i++) {
            if (Players[i].Equals(player)) {
                Debug.Log(Players[i].playerName + " Removed!");
                Players[i] = null;
                return;
            }
        }
        Debug.Log("Removing " + player.playerName + " failed: could not find in a list");
    }

    public static Player[] GetPlayers() {
        return Players;
    }

    public static int NumberOfPlayers() {
        int result = 0;
        for (int i = 0; i < Players.Length; i++) {
            if (Players[i] != null) {
                result++;
            }
        }
        return result;
    }

    public static void ClearIsReady() {
        for (int i = 0; i < Players.Length; i++) {
            if (Players[i] != null) {
                Players[i].isReady = false;
            }
        }
    }

    public void Reset() {
        selectedCharacter = CharacterType.None;
        isReady = false;
        ControllingCharacter = null;
        Kills = 0;
        Deaths = 0;
    }
}
