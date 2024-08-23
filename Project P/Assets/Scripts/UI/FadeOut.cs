using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeOut : MonoBehaviour
{
    public Image panel;
    float time = 0f;
    float F_time = 2f;
    void Start() {
        Init();
        StartCoroutine(FadeFlow());
    }
    void Init() {
        panel = GetComponent<Image>();
        panel.color = new Color(0, 0, 0, 1);
    }
    public void Fade() {
        StartCoroutine(FadeFlow());
    }

    IEnumerator FadeFlow() {
        panel.gameObject.SetActive(true);
        Color alpha = panel.color;
        while (alpha.a > 0f) {
            time += Time.deltaTime / F_time;
            alpha.a = Mathf.Lerp(1, 0, time);
            panel.color = alpha;
            yield return null;
        }
        panel.gameObject.SetActive(false);
        yield return null;
    }
}
