using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Managers : MonoBehaviour
{
    static Managers instance;
    public static Managers Instance { get { Init(); return instance; } }

    DataManager dataManager = new DataManager();
    SoundManager soundManager = new SoundManager();
    GameManager gameManager = new GameManager();
    SceneManager_ sceneManger = new SceneManager_();
    public static DataManager DataManager { get { return Instance.dataManager; } }
    public static SoundManager SoundManager { get { return Instance.soundManager; } }
    public static GameManager GameManager { get { return Instance.gameManager; } }
    public static SceneManager_ SceneManager_ { get { return Instance.sceneManger; } }




    void Start()
    {
        Init();
    }

    static void Init()
    {
        if(instance == null) {
            
            GameObject managers = GameObject.Find("@Managers");
            if(managers == null) {
                managers = new GameObject("@Managers");
                managers.AddComponent<Managers>();
            }
            DontDestroyOnLoad(managers);
            instance = managers.GetComponent<Managers>();
        }
    }
}
