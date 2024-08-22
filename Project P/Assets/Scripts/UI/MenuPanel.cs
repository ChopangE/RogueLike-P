using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPanel : MonoBehaviour
{
    public CanvasGroup mainMenu;
    public RectTransform stageMenu;
    void Start()
    {
        Init();
    }

    void Init() {
        if (SceneManager_.Instance.isFirst) {
            mainMenu.interactable = false;
            mainMenu.alpha = 0.0f;
            mainMenu.blocksRaycasts = false;
            stageMenu.localScale = Vector3.one;
        }
        else {
            mainMenu.interactable = true;
            mainMenu.alpha = 1.0f;
            mainMenu.blocksRaycasts = true;
            stageMenu.localScale = Vector3.zero;
        }
    }
}
