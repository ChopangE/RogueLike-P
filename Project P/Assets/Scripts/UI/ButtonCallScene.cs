using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonCallScene : MonoBehaviour
{
    SceneManager_ scene_;
    public int idx;
    void Start()
    {
        scene_ = FindObjectOfType<SceneManager_>();
    }

    public void CallScene() {
        scene_.CallScene(idx);
    }
}
