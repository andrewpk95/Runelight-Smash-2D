using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ResultManager : MonoBehaviour
{
    public GameObject resultCanvas;
    public Text winnerText; 

    public GameObject KeyboardContinuerPrefab;
    public GameObject JoystickContinuerPrefab;
    public GameObject ResultPanelPrefab;

    UnityAction<Player> OnPlayerJoinAction;
    UnityAction<Player> OnPlayerLeaveAction;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    void Initialize() {
        InitializeManager();
        InitializePlayers();
    }

    void InitializePlayers() {
        // Actual initializer
        Debug.Log("Initializing " + Player.NumberOfPlayers() + " players...");
        if (Player.NumberOfPlayers() <= 0) {
            return;
        }
        foreach (Player player in Player.GetPlayers()) {
            if (player != null) {
                CreatePlayer(player);
            }
        }
    }

    void InitializeManager() {
        OnPlayerJoinAction = new UnityAction<Player>(OnPlayerJoin);
        OnPlayerLeaveAction = new UnityAction<Player>(OnPlayerLeave);
        EventManager.instance.StartListeningToOnPlayerJoinEvent(this.gameObject, OnPlayerJoinAction);
        EventManager.instance.StartListeningToOnPlayerLeaveEvent(this.gameObject, OnPlayerLeaveAction);

        if (GameStateManager.instance.Winners != null && GameStateManager.instance.Winners.Count > 0) {
            string s = "";
            foreach (Player player in GameStateManager.instance.Winners) {
                s += player.playerName + " ";
            }
            winnerText.text = s + "Wins!";
        }
        else {
            winnerText.text = "No winners";
        }
        foreach (Player player in GameStateManager.instance.Rankings.Keys) {
            Debug.Log(GameStateManager.instance.Rankings[player] + "'th place: " + player.playerName);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Return to menu if all players are ready
        if (IsAllPlayerReady()) {
            Player.ClearIsReady();
            PlayerManager.instance.ResetPlayerStates();
            GameManager.instance.LoadMenuScene();
        }
    }

    void CreatePlayer(Player player) {
        IContinuer playerContinuer = Instantiate(KeyboardContinuerPrefab).GetComponent<IContinuer>();
        ResultPanelBahavior resultPanel = Instantiate(ResultPanelPrefab).GetComponent<ResultPanelBahavior>();
        resultPanel.gameObject.name = player.playerName + " Result Panel";
        playerContinuer.ControllingPlayer = player;
        playerContinuer.Panel = resultPanel;
        resultPanel.transform.SetParent(resultCanvas.transform, false);
        resultPanel.controllingPlayer = player;
    }

    void RemovePlayer(Player player) {
        
    }

    void OnPlayerJoin(Player player) {
        Debug.Log(player.playerName + " joined the game!");
        CreatePlayer(player);
    }

    void OnPlayerLeave(Player player) {
        Debug.Log(player.playerName + " left the game!");
        RemovePlayer(player);
    }

    bool IsAllPlayerReady() {
        if (Player.NumberOfPlayers() <= 0) {
            return false;
        }
        foreach (Player p in Player.GetPlayers()) {
            if (p != null) {
                if (!p.isReady) {
                    return false;
                }
            }
        }
        return true;
    }

    void OnDisable() {
        EventManager.instance.UnsubscribeAll(this.gameObject);
    }
}
