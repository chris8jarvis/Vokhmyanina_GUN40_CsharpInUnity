using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] private Transform pointC;
    [SerializeField] private Transform pointD;

    [SerializeField] private float moveDuration = 1f; // длительность между точками
    [SerializeField] private Ease easeType = Ease.Linear;
    [SerializeField] private bool autoPlay = true;

    private List<Transform> waypoints;

    void Start()
    {
        waypoints = new List<Transform> { pointA, pointB, pointC, pointD };
        
        if (autoPlay)
        {
            MoveAlongPath();
        }   
    }

     public void MoveAlongPath()
    {
        if (waypoints.Count == 0) return;

        Sequence pathSequence = DOTween.Sequence();

         foreach (Transform waypoint in waypoints)
        {
            pathSequence.Append(transform.DOMove(waypoint.position, moveDuration)
                .SetEase(easeType));
        }

        pathSequence.SetLoops(-1, LoopType.Yoyo);
    }
}
