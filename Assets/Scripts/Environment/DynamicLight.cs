using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicLight : MonoBehaviour
{
    public Light flickerLight;
    public float minIntensity = 0.5f;
    public float maxIntensity = 1.5f;
    public float speed = 10f;
    
    void Start()
    {
        if (flickerLight == null)
            flickerLight = GetComponent<Light>();
    }
    
    void Update()
    {
        float noise = Mathf.PerlinNoise(Time.time * speed, 0);
        flickerLight.intensity = Mathf.Lerp(minIntensity, maxIntensity, noise);
    }
}
