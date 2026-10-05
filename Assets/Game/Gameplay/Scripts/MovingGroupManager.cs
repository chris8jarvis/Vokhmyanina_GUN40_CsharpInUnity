using System.Collections.Generic;
using UnityEngine;

namespace SampleProject
{
    public sealed class MovingGroupManager : MonoBehaviour
    {
        public void SendGroup(IEnumerable<MoveAgent> agents, Vector3 destination)
        {
            foreach (var agent in agents)
            {
                if (agent != null)
                {
                    agent.SetDestination(destination);
                }
            }
        }
    }
}
