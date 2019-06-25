using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterScreenBackButtonBehavior : MonoBehaviour
{
    bool isLoading;

    public void OnBackButtonClick() {
        if (!isLoading) {
            StartCoroutine(LoadMainMenuScene());
        }
    }

    IEnumerator LoadMainMenuScene() {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync("MenuScene", LoadSceneMode.Single);
        while (!loadOperation.isDone) {
            yield return null;
        }
    }
}
