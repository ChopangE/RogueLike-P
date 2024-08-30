using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class BtnType : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public BTNType currentType;
    public Transform buttonScale;
    Vector3 defaultScale;
    public CanvasGroup mainGroup;
    public CanvasGroup optGroup;
    public RectTransform stagePanel;
    void Start() {
        Init();
    }
    void Init() {
        buttonScale = GetComponent<Transform>();
        defaultScale = buttonScale.localScale;
    }
    bool isSound;
    public void OnBtnClick() {

        switch (currentType) {
            case BTNType.New:
                Managers.DataManager.SetInit();
                panelOn(stagePanel);
                CanvasGroupOff(mainGroup);
                break;
            case BTNType.Continue:
                if (Managers.DataManager.data.level == 0) {
                    Debug.Log("NO Data!");
                }
                else {
                    Managers.DataManager.LoadData();
                    panelOn(stagePanel);
                    CanvasGroupOff(mainGroup);
                }

                break;
            case BTNType.Option:
                CanvasGroupOn(optGroup);
                CanvasGroupOff(mainGroup);
                break;
            case BTNType.Sound:
                CanvasGroupOn(optGroup);
                CanvasGroupOff(mainGroup);
                break;
            case BTNType.Back:
                CanvasGroupOn(mainGroup);
                CanvasGroupOff(optGroup);
                break;
            case BTNType.Quit:
                Managers.DataManager.tmpSave();
                Application.Quit();
                break;
            case BTNType.Status:
                CanvasGroupOn(mainGroup);
                CanvasGroupOff(optGroup);
                break;
        }
    }

    public void CanvasGroupOn(CanvasGroup on) {
        on.interactable = true;
        on.blocksRaycasts = true;
        on.alpha = 1f;
    }
    public void CanvasGroupOff(CanvasGroup on) {
        on.interactable = false;
        on.blocksRaycasts = false;
        on.alpha = 0f;
    }

    public void OnPointerEnter(PointerEventData eventData) {
        buttonScale.localScale = defaultScale * 1.2f;
    }

    public void OnPointerExit(PointerEventData eventData) {
        buttonScale.localScale = defaultScale;
    }
    public void panelOn(RectTransform rect) {
        rect.localScale = Vector3.one;
    }
}
