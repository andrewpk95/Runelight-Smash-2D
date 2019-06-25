using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    Player keyboardPlayer;
    
    // Start is called before the first frame update
    void Awake()
    {
        //Singleton Pattern
        if (instance == null) instance = this;
        else if (instance != this) Destroy(this.gameObject);    
        
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        // Detect joystick controllers joining the game
        foreach (string joystick in Input.GetJoystickNames()) {
            Debug.Log(joystick);
        }
        // Detect Keyboard & Mouse input joining the game
        if (Input.anyKeyDown) {
            if (keyboardPlayer == null) {
                keyboardPlayer = new Player();
                EventManager.instance.InvokeOnPlayerJoinEvent(keyboardPlayer);
            }
        }
        // Test Player De-joining
        if (Input.GetKeyDown(KeyCode.G)) {
            EventManager.instance.InvokeOnPlayerLeaveEvent(keyboardPlayer);
            RemovePlayer(keyboardPlayer);
            keyboardPlayer = null;
        }
    }

    void RemovePlayer(Player player) {
        Player.RemovePlayer(player);
    }

    public void ResetPlayerStates() {
        foreach (Player player in Player.GetPlayers()) {
            if (player != null) {
                player.Reset();
            }
        }
    }
}
