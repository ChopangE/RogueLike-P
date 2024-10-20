using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Stage3 : MonoBehaviour
{
    GameObject player;
    LightController _light; 

    private void Start()
    {
        _light.SpotLight();
    }

    private void Awake()
    {
        Init();
    }

    void Init()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        _light = player.transform.GetChild(4).GetComponent<LightController>();
    }
}
