using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterSelectManager : MonoBehaviour
{
    public static CharacterSelectManager instance;

    public GameObject canvas;
    public GameObject playerCanvas;
    public GameObject readyToFight;

    public GameObject MouseCursorPrefab;
    public GameObject JoystickCursorPrefab;
    public GameObject TokenPrefab;
    public GameObject PlayerPanelPrefab;

    // Start is called before the first frame update
    void Start()
    {
        //Singleton Pattern
        if (instance == null) instance = this;
        else if (instance != this) Destroy(this.gameObject);    
        
        Initialize();
    }

    void Initialize() {
        InitializeManager();
        InitializePlayers();
    }

    void InitializePlayers() {
        // Actual initializer
        Debug.Log("Initializing " + Player.NumberOfPlayers() + " players...");
        foreach (Player player in Player.GetPlayers()) {
            if (player != null) {
                CreatePlayer(player);
            }
        }
        // Default initializer
        if (Player.NumberOfPlayers() <= 1) {
            Debug.Log("Initializing default players...");
            CreatePlayer(new Player(true));
            return;
        }
    }

    void InitializeManager() {
        canvas = GameObject.FindGameObjectWithTag("Canvas");

        EventManager.instance.StartListeningToOnPlayerJoinEvent(new UnityAction<Player>(OnPlayerJoin));
        EventManager.instance.StartListeningToOnPlayerLeaveEvent(new UnityAction<Player>(OnPlayerLeave));
        EventManager.instance.StartListeningToOnCharacterSelectEvent(new UnityAction<Player, CharacterType>(OnCharacterSelect));
        EventManager.instance.StartListeningToOnCharacterDeSelectEvent(new UnityAction<Player>(OnCharacterDeSelect));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CreatePlayer(Player player) {
        ICursor playerCursor = Instantiate(MouseCursorPrefab).GetComponent<ICursor>();
        Token playerToken = Instantiate(TokenPrefab).GetComponent<Token>();
        PlayerPanelBehavior playerPanel = Instantiate(PlayerPanelPrefab).GetComponent<PlayerPanelBehavior>();

        playerToken.transform.SetParent(canvas.transform, false);
        playerToken.name = player.playerName + " Token";

        playerCursor.ControllingPlayer = player;
        playerCursor.Token = playerToken;

        playerPanel.transform.SetParent(playerCanvas.transform, false);
        playerPanel.SetPlayer(player);
    }

    void RemovePlayer(Player player) {
        
    }

    void OnPlayerJoin(Player player) {
        Debug.Log(player.playerName + " joined the game!");
        CreatePlayer(player);
        IsAllPlayerReady();
    }

    void OnPlayerLeave(Player player) {
        Debug.Log(player.playerName + " left the game!");
        RemovePlayer(player);
        IsAllPlayerReady();
    }

    void OnCharacterSelect(Player player, CharacterType character) {
        Debug.Log(player.playerName + " selected " + character);
        player.selectedCharacter = character;
        IsAllPlayerReady();
    }

    void OnCharacterDeSelect(Player player) {
        Debug.Log(player.playerName + " dropped selection");
        player.selectedCharacter = CharacterType.None;
        IsAllPlayerReady();
    }

    bool IsAllPlayerReady() {
        foreach (Player p in Player.GetPlayers()) {
            if (p != null) {
                if (p.selectedCharacter == CharacterType.None) {
                    readyToFight.SetActive(false);
                    return false;
                }
            }
        }
        readyToFight.SetActive(true);
        return true;
    }
}
