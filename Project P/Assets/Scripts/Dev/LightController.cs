using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightController : MonoBehaviour
{
    Light2D light2d;
    Coroutine coWait;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        light2d = this.GetComponent<Light2D>();
        light2d.lightType = Light2D.LightType.Global; 
    }

    private void Update()
    {
        
    }

    public void SpotLight()
    {
        light2d.lightType = Light2D.LightType.Point;
        
        light2d.pointLightInnerRadius = 2f;
        light2d.pointLightOuterRadius = 3.5f;
        light2d.pointLightInnerAngle = 360f;
        light2d.pointLightOuterAngle = 360f; 
    }

    void Global()
    {
        light2d.lightType = Light2D.LightType.Global;
    }

    public void StartSpotLight(float seconds)
    {
        CancelSpotLight(); 
        coWait = StartCoroutine("CoSpot", seconds);
    }

    void CancelSpotLight()
    {
        if (coWait == null)
            return;

        StopCoroutine(coWait); 
        coWait = null; 
    }

    IEnumerator CoSpot(float seconds)
    {
        SpotLight(); 
        yield return new WaitForSeconds(seconds);
        coWait = null;
        Global(); 
    }

}
