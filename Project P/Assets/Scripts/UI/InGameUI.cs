using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    [Header ("#UI")]
    public Text Level;
    public Text Hp;
    public Slider HpSlider;
    public void Start()
    {
        PlayerControl player = FindObjectOfType<PlayerControl>();
        Notifying();
    }
    
    public void Notifying() {
        Level.text = "Lv   " + GameManager.instance.pd.level;
        Hp.text = "HP  " + GameManager.instance.pd.curhealth + "  /  " + GameManager.instance.pd.health;
        HpSlider.value = (float)GameManager.instance.pd.curhealth / GameManager.instance.pd.health;
    }

    private void Update() {
        //HpSlider.value = (float)GameManager.instance.pd.curhealth / GameManager.instance.pd.health;
    }


    void UIHelathUpdate()
    {
        HpSlider.value = (float)GameManager.instance.pd.curhealth / GameManager.instance.pd.health;
        Hp.text = "HP  " + GameManager.instance.pd.curhealth + "  /  " + GameManager.instance.pd.health;
    }
   
}
