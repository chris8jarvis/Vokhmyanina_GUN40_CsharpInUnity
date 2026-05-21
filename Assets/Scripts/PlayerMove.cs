using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

public class PlayerMove : MonoBehaviour
{

    //пункты для движения к ним
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] private Transform pointC;
    [SerializeField] private Transform pointD;
    
    //настройки движения
    [SerializeField] private float moveDuration = 1f; // длительность между точками
    [SerializeField] private Ease easeType = Ease.Linear;
    [SerializeField] private bool autoPlay = true;

    //настройки масштабирования
    [SerializeField] private float scaleUpDuration = 0.3f;
    [SerializeField] private float scaleDownDuration = 0.3f;
    [SerializeField] private Vector3 scaleTarget = new Vector3(1.2f, 1.2f, 1.2f);

    //настройки цвета
    [SerializeField] private Material playerMaterial;
    [SerializeField] private Color defaultColor = Color.white;
    [SerializeField] private Color pointBColor = Color.red;
    [SerializeField] private Color pointCColor = Color.blue;

    //сохраняем точки-пункты 
    private List<Transform> waypoints;

    //словарь для запоминания цвета пунктов
    private Dictionary<Transform, Color> waypointColors;
    private Renderer playerRenderer;

    void Start()
    {
        waypoints = new List<Transform> { pointA, pointB, pointC, pointD }; //список точек в порядке движения сферы

        playerRenderer = GetComponent<Renderer>();
        if (playerMaterial == null && playerRenderer != null)
            playerMaterial = playerRenderer.material;

        AssignColorsToCubes();
        
        if (autoPlay)
        {
            MoveAlongPath();
        }   
    }

    //красим кубы в соответствующие цвета
    void AssignColorsToCubes()
    {
        if (pointB != null)
        {
            Renderer cubeRenderer = pointB.GetComponent<Renderer>();
            if (cubeRenderer != null)
                cubeRenderer.material.color = pointBColor;
        }
        
        if (pointC != null)
        {
            Renderer cubeRenderer = pointC.GetComponent<Renderer>();
            if (cubeRenderer != null)
                cubeRenderer.material.color = pointCColor;
        }
    }

    //движение по маршруту
     public void MoveAlongPath()
    {
        if (waypoints.Count == 0) return;

        Sequence pathSequence = DOTween.Sequence();

         foreach (Transform waypoint in waypoints)
        {
            pathSequence.Append(transform.DOMove(waypoint.position, moveDuration)
                .SetEase(easeType));

                if (waypoint == pointB)
            {
                pathSequence.AppendCallback(() => OnReachPointB());
            }
            else if (waypoint == pointC)
            {
                pathSequence.AppendCallback(() => OnReachPointC());
            }
        }
        pathSequence.OnComplete(() => {
            ChangeColor(defaultColor);
        });

        pathSequence.SetLoops(-1, LoopType.Yoyo);
    }

    //при достижении точки-пункта "B", модификации сферы 
     void OnReachPointB()
    {
        Sequence effectSequence = DOTween.Sequence();
        
        effectSequence.Append(transform.DOScale(scaleTarget, scaleUpDuration))
                      .Join(ChangeColorWithTween(pointBColor, 0.3f))
                      .Append(transform.DOScale(Vector3.one, scaleDownDuration));
        
        effectSequence.Play();
    }

    //при достижении точки-пункта "C", модификации сферы 
    void OnReachPointC()
    {
        Sequence effectSequence = DOTween.Sequence();
        
        effectSequence.Append(transform.DOScale(scaleTarget, scaleUpDuration))
                      .Join(ChangeColorWithTween(pointCColor, 0.3f))
                      .Append(transform.DOScale(Vector3.one, scaleDownDuration));
        
        effectSequence.Play();
    }

    //смена цвета
    void ChangeColor(Color newColor)
    {
        if (playerMaterial != null)
            playerMaterial.color = newColor;
    }

    //плавная смена цвета    
    Tweener ChangeColorWithTween(Color newColor, float duration)
    {
        if (playerMaterial != null)
            return playerMaterial.DOColor(newColor, duration);
        return null;
    }
}
