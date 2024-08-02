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
                DataManager.Instance.SetInit();
                panelOn(stagePanel);
                CanvasGroupOff(mainGroup);
                break;
            case BTNType.Continue:
                if (DataManager.Instance.data.level == 0) {
                    Debug.Log("NO Data!");
                }
                else {
                    DataManager.Instance.LoadContinue();
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
                DataManager.Instance.tmpSave();
                Application.Quit();
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
