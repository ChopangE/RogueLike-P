using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ButtonCallScene : MonoBehaviour
{
    SceneManager_ scene_;
    Button button;
    int idx = 1;
    void Start()
    {
        scene_ = FindObjectOfType<SceneManager_>();
        //Init();
    }

    public void CallScene() {
        scene_.CallScene(1);
    }
    void Init() {
        button = GetComponent<Button>();
        if(idx == DataManager.Instance.data.curStage) {
            button.interactable = true;
        }
        else {
            button.interactable = false;

        }
    }
}
