using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Map7 : MonoBehaviour
{
    GameObject player;
    LightController _light; 

    private void OnEnable()
    {
        _light.SpotLight(); 
    }

    void Init()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        _light = player.transform.GetChild(4).GetComponent<LightController>(); 
    }
}
