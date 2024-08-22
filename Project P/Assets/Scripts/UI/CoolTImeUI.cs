using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoolTImeUI : MonoBehaviour
{
    public enum CoolType { Atk, Dash};
    public CoolType ct;
    public Image image;

    PlayerControl pc;
    float timer;
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        switch (ct) {
            case CoolType.Atk:
                if (pc.attackEnable) {
                    timer = 0f;
                    image.fillAmount = 0f;
                    return;
                }
                StartCoroutine(StartAtkCoolTime());
                break;
            case CoolType.Dash:
                if (pc.dashEnable) {
                    timer = 0f;
                    image.fillAmount = 0f;
                    return;
                }
                StartCoroutine(StartDashCoolTime());
                break;

        }
    }

    void Init() {
        pc = FindObjectOfType<PlayerControl>();
        image = GetComponent<Image>();
        timer = 0f;
        image.fillAmount = 0f;
    }

    IEnumerator StartAtkCoolTime() {
        yield return null;
        float coolTime = pc.attackCurTime;
        timer += Time.deltaTime / coolTime;
        image.fillAmount = Mathf.Lerp(1, 0, timer);
    }

    IEnumerator StartDashCoolTime() {
        yield return null;
        float coolTime = pc.dashCurTime;
        timer += Time.deltaTime / coolTime;
        image.fillAmount = Mathf.Lerp(1, 0, timer);
    }
}
