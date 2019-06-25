using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FighterPanelBahavior : MonoBehaviour
{
    public Player controllingPlayer;
    public IDamageable fighter;

    public Image fighterImage;
    public Text percentageText;
    public Text fighterName;

    public Sprite Ron;

    // Start is called before the first frame update
    void Start()
    {
        fighterImage.sprite = Ron;
        fighter = controllingPlayer.ControllingCharacter.GetComponent<IDamageable>();
        percentageText.text = string.Format("{0:0.0}%", fighter.Percentage);
        fighterName.text = controllingPlayer.selectedCharacter.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        percentageText.text = string.Format("{0:0.0}%", fighter.Percentage);
    }
}
