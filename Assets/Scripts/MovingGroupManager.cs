using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingGroupManager : MonoBehaviour
{
    [Header("Prefab & Pool")]
    [SerializeField] private ObjectsMover _agentPrefab;
    [SerializeField] private int _poolSize = 50;

    [Header("Spawn Settings")]
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _lifeTime = 3f;
    [SerializeField] private float _spawnRadius = 10f;

    private readonly Queue<MoveAgent> _pool = new Queue<MoveAgent>();
    private readonly List<MovingGroupAgent> _groups = new List<MovingGroupAgent>();

    private void Awake()
    {
        CreatePool();
    }

    private void CreatePool()
    {
        for (int i = 0; i < _poolSize; i++)
        {
            var mover = Instantiate(_agentPrefab, transform);
            mover.gameObject.SetActive(false);
            _pool.Enqueue(new MoveAgent(mover));
        }
    }
    private MoveAgent GetFromPool()
    {
        if (_pool.Count == 0)
        {
            Debug.LogWarning("Pool is empty");
            return null;
        }
        return _pool.Dequeue();
    }
    private void ReturnToPool(MoveAgent agent)
    {
        agent.Mover.Deactivate();
        _pool.Enqueue(agent);
    }
    public void SpawnGroup(int count, Vector3 center)
    {
        var group = new MovingGroupAgent
        {
            GroupDirection = Algorithms.GetRandomDirectionXZ(),
            GroupSpeed = _speed
        };

        for (int i = 0; i < count; i++)
        {
            var agent = GetFromPool();
            if (agent == null) break;

            Vector3 spawnPos = Algorithms.GetPointOnCircle(center, _spawnRadius);
            agent.Direction = group.GroupDirection;
            agent.Speed = group.GroupSpeed;
            agent.LifeTime = _lifeTime;

            agent.Mover.Init(spawnPos, agent.Direction, agent.Speed, agent.LifeTime);
            group.Agents.Add(agent);
        }

        _groups.Add(group);
    }

    private void Update()
    {
        for (int i = _groups.Count - 1; i >= 0; i--)
        {
            var group = _groups[i];
            for (int j = group.Agents.Count - 1; j >= 0; j--)
            {
                if (!group.Agents[j].IsBusy)
                {
                    ReturnToPool(group.Agents[j]);
                    group.Agents.RemoveAt(j);
                }
            }
            if (group.IsEmpty) _groups.RemoveAt(i);
        }
    }
}