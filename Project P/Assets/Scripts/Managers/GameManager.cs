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
    bool isLive;
    void Awake() {
        instance = this;
    }

    void Start() {
        Init();
    }

    void Init() {
        player = FindAnyObjectByType<PlayerControl>()   ;
        pd = DataManager.Instance.GetData();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            uiPause.Show();
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
        DataManager.Instance.tmpSave();
    }
}
