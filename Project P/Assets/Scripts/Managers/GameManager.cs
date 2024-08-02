using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    DataManager dataManager;
    PlayerControl player;
    public PauseUI uiPause;
    public static GameManager instance;
    bool isLive;
    void Awake() {
        instance = this;
    }

    void Start() {
        Init();
    }

    void Init() {
        dataManager = DataManager.Instance;
        player = FindAnyObjectByType<PlayerControl>();
//uiPause = FindAnyObjectByType<PauseUI>();
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

}
