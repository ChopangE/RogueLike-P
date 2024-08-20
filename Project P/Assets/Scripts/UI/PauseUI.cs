using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseUI : MonoBehaviour
{
    public RectTransform rect;
    void Awake() {
        rect = GetComponent<RectTransform>();
    }
    
    public void Show() {
        rect.localScale = Vector3.one;
        GameManager.instance.Stop();
        SoundManager.Instance.EffectBgm(true);
    }
    public void Hide() {
        rect.localScale = Vector3.zero;
        GameManager.instance.Resume();
        SoundManager.Instance.EffectBgm(false);

    }
}
