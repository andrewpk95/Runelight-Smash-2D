using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameRuleManager : MonoBehaviour
{
    public const int CHECK_WINNER_INTERVAL = 300;

    public GameObject canvas;

    public GameObject KeyboardControllerPrefab;
    public GameObject Xbox360ControllerPrefab;
    public GameObject AIControllerPrefab;

    public GameObject FighterPanelPrefab;

    public GameObject RonPrefab;

    protected GameObject[] spawnPointList;
    protected int spawnPointIndex;
    
    IGameRule gameRule;
    int interval;

    List<IController> controllers;
    
    // Start is called before the first frame update
    void Awake()
    {
        controllers = new List<IController>();

        Initialize();
    }

    void Start() {
        gameRule.StartGame();
    }

    void Initialize() {
        InitializePlayers();
        InitializeGameRule();
    }

    void InitializeGameRule() {
        interval = CHECK_WINNER_INTERVAL;
        gameRule = GameStateManager.instance.GameRule;
        EventManager.instance.StartListeningToOnGameOverEvent(new UnityAction(OnGameOver));
    }

    void InitializePlayers() {
        spawnPointList = GameObject.FindGameObjectsWithTag("SpawnPoint");
        System.Array.Sort(spawnPointList, delegate(GameObject x, GameObject y) {return x.name.CompareTo(y.name);});
        // Default initializer
        if (Player.NumberOfPlayers() <= 0) {
            Debug.Log("Initializing default players...");

            Player player = new Player();
            CreatePlayer(player);

            Player player2 = new Player(true);
            CreatePlayer(player2);

            return;
        }
        // Actual initializer
        Debug.Log("Initializing " + Player.NumberOfPlayers() + " players...");
        foreach (Player player in Player.GetPlayers()) {
            if (player != null) {
                CreatePlayer(player);
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        interval--;
        if (interval < 0) {
            string log = "Current Winners: ";
            foreach (Player player in gameRule.GetCurrentWinners()) {
                log = log + player.playerName + " ";
            }
            Debug.Log(log);
            interval = CHECK_WINNER_INTERVAL;
        }
    }

    void CreatePlayer(Player player) {
        IController playerController;
        if (player.isComputer) {
            playerController = Instantiate(AIControllerPrefab).GetComponent<IController>();
        }
        else {
            playerController = Instantiate(KeyboardControllerPrefab).GetComponent<IController>();
        }
        controllers.Add(playerController);
        GameObject playerFighter = Instantiate(RonPrefab);

        player.ControllingCharacter = playerFighter;
        
        playerFighter.name = player.playerName + " " + RonPrefab.name;
        if (spawnPointIndex == spawnPointList.Length) {
            spawnPointIndex = 0;
        }
        playerFighter.transform.position = spawnPointList[spawnPointIndex].transform.position;
        spawnPointIndex++;

        playerController.ControllingPlayer = player;
        playerController.Fighter = playerFighter;

        FighterPanelBahavior fighterPanel = Instantiate(FighterPanelPrefab).GetComponent<FighterPanelBahavior>();
        fighterPanel.controllingPlayer = player;
        fighterPanel.transform.SetParent(canvas.transform, false);
    }

    void RemovePlayer(Player player) {

    }

    void OnGameOver() {
        foreach (IController controller in controllers) {
            
        }
        gameRule.StopGame();
        GameStateManager.instance.Winners = gameRule.GetCurrentWinners();
        GameStateManager.instance.Rankings = gameRule.GetRankings();
        GameManager.instance.LoadResultScene();
    }
}
