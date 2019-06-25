using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SmashButtonBehavior : MonoBehaviour
{
    bool isLoading;

    public void OnSmashButtonClick() {
        if (!isLoading) {
            GameManager.instance.LoadCharacterSelectScene();
        }
    }
}
