using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingGroupAgent
{
    public List<MoveAgent> Agents = new List<MoveAgent>();
    public Vector3 GroupDirection;
    public float GroupSpeed;

    public bool IsEmpty => Agents.Count == 0;
}