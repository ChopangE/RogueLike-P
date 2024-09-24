using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageButtonManager : MonoBehaviour
{
    public Button[] buttons;
    DataManager dataManager;
    void Start() {
        dataManager = FindObjectOfType<DataManager>();
        for(int i = 0; i < buttons.Length; i++) { 
            if(i == dataManager.data.curStage) {
                buttons[i].interactable = true;
            }
            else {
                buttons[i].interactable = false;

            }

        }
    }
}
