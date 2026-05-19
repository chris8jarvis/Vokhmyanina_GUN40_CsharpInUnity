using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIBaseState
{
    protected AIAgent agent;
    
    public AIBaseState(AIAgent agent)
    {
        this.agent = agent;
    }
    
    public abstract void OnEnter();
    public abstract void OnUpdate();
    public abstract void OnExit();
}
