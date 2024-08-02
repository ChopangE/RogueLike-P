using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    DataManager dataManager;
    PlayerControl player;

    void Awake() {
        dataManager = DataManager.Instance;
        player = FindAnyObjectByType<PlayerControl>();
    }


}
