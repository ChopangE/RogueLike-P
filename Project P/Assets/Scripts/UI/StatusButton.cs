using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public enum statType { health, atk, speed, jump, level, statPoint };

public class StatusButton : MonoBehaviour
{

    public statType st;
    public TextMeshProUGUI text;
    public StatusButton targetPrint;
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    void Init() {
        switch (st) {
            //각자 남아있는 스킬포인트 확인하고 Up
            case statType.health:
                text.text = (GameManager.instance.pd.health) + "";
                break;
            case statType.atk:
                text.text = (GameManager.instance.pd.atk) + "";
                break;
            case statType.speed:
                text.text = (GameManager.instance.pd.speed) + "";
                break;
            case statType.jump:
                text.text = (GameManager.instance.pd.jump) + "";
                break;
            case statType.level:
                text.text = "Lv : " + (GameManager.instance.pd.level) + "";
                break;
            case statType.statPoint:
                text.text = "Stat Point : " + (GameManager.instance.pd.statPoint) + "";
                break;
        }
    }
    public void OnButtonClicked() {
        if (GameManager.instance.pd.statPoint <= 0) return;
            switch (st) {
            //각자 남아있는 스킬포인트 확인하고 Up
            case statType.health:
                GameManager.instance.pd.statPoint--;
                text.text= (++GameManager.instance.pd.health) + "";
                Notifying();
                break;
            case statType.atk:
                GameManager.instance.pd.statPoint--;
                text.text = (++GameManager.instance.pd.atk) + "";
                Notifying();
                break;
            case statType.speed:
                GameManager.instance.pd.statPoint--;
                text.text = (++GameManager.instance.pd.speed) + "";
                Notifying();
                break;
            case statType.jump:
                GameManager.instance.pd.statPoint--;
                Notifying();
                text.text = (++GameManager.instance.pd.jump) + "";
                break;
            } 
    }

    public void Notifying() {
        targetPrint.notified();
    }
    public void notified() {
        switch (st) {
            case statType.statPoint:
                text.text = "Stat Point : " + (GameManager.instance.pd.statPoint) + "";
                break;
            case statType.level:
                text.text = "Lv : " + (GameManager.instance.pd.level) + "";
                break;
        }
    }
}
