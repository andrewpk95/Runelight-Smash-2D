using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ResultPanelState {
    Overall,
    Detailed,
    ReadyForNextBattle
}

public class ResultPanelBahavior : MonoBehaviour
{
    public Player controllingPlayer;
    public ResultPanelState currentState;

    public GameObject overallPanel;
    public Text overallPlayerNumber;
    public Image overallCharacterImage;
    public Text overallCharacterName;
    public Text overallPlayerName;
    public Image overallRankImage;

    public GameObject detailedPanel;
    public Image detailedCharacterImage;
    public Text detailedCharacterName;
    public Text detailedPlayerName;
    public Text killText;
    public Text deathText;
    public Text totalText;
    public Image detailedRankImage;

    public GameObject readyForNextBattlePanel;

    public Sprite Ron;

    GameObject[] screens;

    // Start is called before the first frame update
    void Start()
    {
        screens = new GameObject[] {overallPanel, detailedPanel, readyForNextBattlePanel};
        currentState = ResultPanelState.Overall;
        overallPanel.SetActive(true);

        overallPlayerNumber.text = "P" + controllingPlayer.playerNumber;
        overallCharacterImage.sprite = Ron;
        overallCharacterName.text = controllingPlayer.selectedCharacter.ToString();
        overallPlayerName.text = controllingPlayer.playerName;
        overallRankImage.sprite = Ron;

        detailedCharacterImage.sprite = Ron;
        detailedCharacterName.text = controllingPlayer.selectedCharacter.ToString();
        detailedPlayerName.text = controllingPlayer.playerName;
        killText.text = "Kills: " + controllingPlayer.Kills;
        deathText.text = "Deaths: " + controllingPlayer.Deaths;
        totalText.text = "Total: " + controllingPlayer.Total;
        detailedRankImage.sprite = Ron;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NextScreen() {
        if (currentState != ResultPanelState.ReadyForNextBattle) {
            screens[(int) currentState].SetActive(false);
            currentState++;
            screens[(int) currentState].SetActive(true);
        }
        if (currentState == ResultPanelState.ReadyForNextBattle) {
            controllingPlayer.isReady = true;
        }
    }

    public void PrevScreen() {
        if (currentState != ResultPanelState.Overall) {
            screens[(int) currentState].SetActive(false);
            currentState--;
            screens[(int) currentState].SetActive(true);
        }
        controllingPlayer.isReady = false;
    }
}
