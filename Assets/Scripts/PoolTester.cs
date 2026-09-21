using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolTester : MonoBehaviour
{
    [SerializeField] private MovingGroupManager _manager;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _manager.SpawnGroup(10, Vector3.zero);
        }
    }
}
