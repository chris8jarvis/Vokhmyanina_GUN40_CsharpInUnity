using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Algorithms
{
    public static Vector3 GetDirection(Vector3 from, Vector3 to)
    {
        return (to - from).normalized;
    }
    public static Vector3 GetRandomDirectionXZ()
    {
        float angle = Random.Range(0f, Mathf.PI * 2f);
        return new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle));
    }
    public static Vector3 GetPointOnCircle(Vector3 center, float radius)
    {
        float angle = Random.Range(0f, Mathf.PI * 2f);
        return center + new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)) * radius;
    }
}
