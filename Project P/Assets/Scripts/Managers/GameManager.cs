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
    public RectTransform gameOver;
    public InGameUI inGameUI;
    Transform starting;
    bool isLive;
    bool isUIOn;
    void Awake() {
        instance = this;
        Init();
    }

    void Start() {
        //Init();
    }

    void Init() {
        player = FindAnyObjectByType<PlayerControl>();
        pd = DataManager.Instance.GetData();
        isUIOn = false;
        Resume();
        MapsOn();
        SetStatus();
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
        if (player.curHealth <= 0) {
            isLive = false;
            Animator anim = player.GetComponent<Animator>();
            if(anim.GetCurrentAnimatorStateInfo(0).IsName("Death NoEffect") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f) {
                GameOver();
            }
        }
        if (!isLive) return;
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
        //isLive = false;
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
    public void SetStatus() {
        player.SetStatus();
        inGameUI.Notifying();
    }

    public void GameOver() {
        
        Stop();
        gameOver.localScale = Vector3.one;
    }

    public void GoToHome() {
        gameOver.localScale = Vector3.zero;
        FindObjectOfType<DataManager>().SetInit(); 
        FindObjectOfType<SceneManager_>().CallMainScene();
        //초기화해야됨 여기서 
    }

}
