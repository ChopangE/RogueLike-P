using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public enum statType { health, atk, speed, jump };

public class StatusButton : MonoBehaviour
{

    public statType st;
    public TextMeshProUGUI text;
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
        }
    }
    public void OnButtonClicked() {
        if (GameManager.instance.pd.statPoint <= 0) return;
            switch (st) {
            //각자 남아있는 스킬포인트 확인하고 Up
            case statType.health:
                GameManager.instance.pd.statPoint--;
                text.text= (++GameManager.instance.pd.health) + "";
                
                break;
            case statType.atk:
                GameManager.instance.pd.statPoint--;
                text.text = (++GameManager.instance.pd.atk) + "";
                break;
            case statType.speed:
                GameManager.instance.pd.statPoint--;
                text.text = (++GameManager.instance.pd.speed) + "";
                break;
            case statType.jump:
                GameManager.instance.pd.statPoint--;
                text.text = (++GameManager.instance.pd.jump) + "";
                break;
            } 
    }
}
