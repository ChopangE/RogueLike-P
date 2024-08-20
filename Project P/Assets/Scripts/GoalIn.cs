using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalIn : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision) {
        if (!collision.CompareTag("Player")) return;
        Debug.Log("Why?");
        GameManager.instance.StageClear();
    }
}
