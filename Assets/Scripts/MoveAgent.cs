using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAgent
{
    public ObjectsMover Mover;
    public Vector3 Direction;
    public float Speed;
    public float LifeTime;

    public MoveAgent(ObjectsMover mover)
    {
        Mover = mover;
    }

    public bool IsBusy => Mover != null && Mover.IsActive;
}