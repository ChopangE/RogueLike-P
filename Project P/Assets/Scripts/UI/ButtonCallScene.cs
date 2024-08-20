using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ButtonCallScene : MonoBehaviour
{
    SceneManager_ scene_;
    Button button;
    public int idx;
    void Start()
    {
        scene_ = FindObjectOfType<SceneManager_>();
        Init();
    }

    public void CallScene() {
        scene_.CallScene(0);
    }
    void Init() {
        button = GetComponent<Button>();
        button.interactable = (idx == DataManager.Instance.data.curStage);
    }
}
