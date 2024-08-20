using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DataManager;

public class GameManager : MonoBehaviour
{
    PlayerControl player;
    public PauseUI uiPause;
    public static GameManager instance;
    public PlayerData pd;
    public GameObject[] maps;
    Transform starting;
    bool isLive;
    bool isUIOn;
    void Awake() {
        instance = this;
    }

    void Start() {
        Init();
    }

    void Init() {
        player = FindAnyObjectByType<PlayerControl>();
        pd = DataManager.Instance.GetData();
        isUIOn = false;
        MapsOn();
    }

    void MapsOn() {
        for(int i = 0; i < maps.Length; i++) {
            if (i == pd.curStage) maps[i].gameObject.SetActive(true);
            else maps[i].gameObject.SetActive(false);
        }
        starting = GameObject.FindWithTag("StartingPoint").transform;
        player.transform.position = starting.position;
    }


    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape) && !isUIOn) {
            uiPause.Show();
            isUIOn = true;
        }
        else if(Input.GetKeyDown(KeyCode.Escape) && isUIOn) {
            uiPause.Hide();
            isUIOn = false;
        }
    }
    public void Stop() {
        isLive = false;
        Time.timeScale = 0;
    }

    public void Resume() {

        isLive = true;
        Time.timeScale = 1;

    }
    public void StageClear() {
        PlayerLevelUp();
        DataManager.Instance.SetDataAndSave(pd);
        SceneManager_.Instance.CallScene(1);
    }
    void PlayerLevelUp() {
        pd.level++;
        pd.statPoint++;
        pd.curStage++;
    }
}
