using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public bool isLoading;

    public GameObject InputManagerPrefab;
    public GameObject TimerPrefab;
    public GameObject EventManagerPrefab;
    public GameObject PlayerManagerPrefab;
    public GameObject GameStateManagerPrefab;

    // Start is called before the first frame update
    void Awake()
    {
        //Singleton Pattern
        if (instance == null) instance = this;
        else if (instance != this) Destroy(this.gameObject);    
        
        DontDestroyOnLoad(this.gameObject);
        
        Initialize();
    }

    void Initialize() {
        Instantiate(EventManagerPrefab);
        Instantiate(TimerPrefab);
        Instantiate(InputManagerPrefab);
        Instantiate(PlayerManagerPrefab);
        Instantiate(GameStateManagerPrefab);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadMenuScene() {
        if (!isLoading) {
            Debug.Log("Start Loading Menu Scene...");
            ResetManagers();
            isLoading = true;
            StartCoroutine(LoadScene("MenuScene"));
        }
    }

    public void LoadCharacterSelectScene() {
        if (!isLoading) {
            Debug.Log("Start Loading Character Select Scene...");
            ResetManagers();
            isLoading = true;
            StartCoroutine(LoadScene("CharacterSelectScene"));
        }
    }

    public void LoadBattleScene() {
        if (!isLoading) {
            Debug.Log("Start Loading Battle Scene...");
            ResetManagers();
            isLoading = true;
            StartCoroutine(LoadScene("BattleScene"));
        }
    }

    public void LoadResultScene() {
        if (!isLoading) {
            Debug.Log("Start Loading Result Scene...");
            ResetManagers();
            isLoading = true;
            StartCoroutine(LoadScene("ResultScene"));
        }
    }

    public void ResetManagers() {
        TimerManager.instance.Reset();
        //GameStateManager.instance.Reset();
    }

    IEnumerator LoadScene(string scene) {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Single);
        while (!loadOperation.isDone) {
            yield return null;
        }
        isLoading = false;
    }
}
