using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerPanelBehavior : MonoBehaviour
{
    public Player controllingPlayer;
    public Sprite sprite;

    public Image image;
    public Text characterNameText;
    public Text playerNameText;
    public Image controllerImage;
    public Text playerNumberText;

    // Start is called before the first frame update
    void Start()
    {
        EventManager.instance.StartListeningToOnCharacterSelectEvent(this.gameObject, new UnityAction<Player, CharacterType>(OnCharacterSelect));
        EventManager.instance.StartListeningToOnCharacterDeSelectEvent(this.gameObject, new UnityAction<Player>(OnCharacterDeSelect));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCharacterSelect(Player player, CharacterType character) {
        if (player.Equals(controllingPlayer)) {
            SetCharacter(character);
        }
    }

    void OnCharacterDeSelect(Player player) {
        if (player.Equals(controllingPlayer)) {
            SetCharacter(CharacterType.None);
        }
    }

    void SetCharacter(CharacterType character) {
        characterNameText.text = character.ToString();
        switch(character) {
            case CharacterType.Ron:
                image.sprite = sprite;
                break;
            default:
                image.sprite = null;
                break;
        }
    }

    public void SetPlayer(Player player) {
        controllingPlayer = player;
        playerNameText.text = controllingPlayer.playerName;
        if (controllingPlayer.isComputer) {
            playerNumberText.text = string.Format("CPU");
        }
        else {
            playerNumberText.text = string.Format("P{0}", controllingPlayer.playerNumber);
        }
        
    }

    void OnDisable() {
        EventManager.instance.UnsubscribeAll(this.gameObject);
    }
}
