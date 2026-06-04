using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    public Light sunLight;
    public Light moonLight;
    public float intensitySun = 1f;
    public float intensityMoon = 0.2f;
    
    private bool isDay = true;
    
    void Start()
    {
        SetDay();
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            SetDay();
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            SetNight();
        }
    }
    
    public void SetDay()
    {
        isDay = true;
        sunLight.intensity = intensitySun;
        if (moonLight != null) moonLight.intensity = 0;
        Debug.Log("Дневное освещение");
    }
    
    public void SetNight()
    {
        isDay = false;
        sunLight.intensity = 0;
        if (moonLight != null) moonLight.intensity = intensityMoon;
        Debug.Log("Ночное освещение");
    }
}
