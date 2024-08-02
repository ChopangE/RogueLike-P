using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BtnType : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public BTNType currentType;
    public Transform buttonScale;
    Vector3 defaultScale;
    public CanvasGroup mainGroup;
    public CanvasGroup optGroup;

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
                break;
            case BTNType.Continue:
                DataManager.Instance.LoadContinue();
                break;
            case BTNType.Option:
                CanvasGroupOn(optGroup);
                CanvasGroupOff(mainGroup);
                break;
            case BTNType.Sound:
                //����
                break;
            case BTNType.Back:
                CanvasGroupOn(mainGroup);
                CanvasGroupOff(optGroup);
                break;
            case BTNType.Quit:
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
}
