using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager instance;

    public StageType SelectedStage;
    public IGameRule GameRule;
    public const int DEFAULT_TIME = 30;

    public List<Player> Winners;
    public Dictionary<Player, int> Rankings;

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
        GameRule = new TimeGameRule(DEFAULT_TIME);
        SelectedStage = StageType.BATTLE_FIELD;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Reset() {
        GameRule = new TimeGameRule(5);
        SelectedStage = StageType.BATTLE_FIELD;
    }
}
