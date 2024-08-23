using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Map8 : MonoBehaviour
{
    public GameObject canvas;

    public GameObject bossObject;
    public BringerOfDeath boss; 

    private void OnEnable()
    {
        Init();

        StartCoroutine(coWait()); 
    }

    void Init()
    {
        canvas.SetActive(false);
        bossObject.SetActive(true); 

        boss = bossObject.GetComponent<BringerOfDeath>();
        boss.UpdateBossReady(false);

    }

    IEnumerator coWait()
    {
        yield return new WaitForSeconds(1f);
        
        boss.UpdateBossReady(true);
        canvas.SetActive(true); 

    }
}
