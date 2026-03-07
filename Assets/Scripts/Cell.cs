using UnityEngine.EventSystems;
using UnityEngine;
using Controllers;
using System;
using Units;

public class Cell : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private GameObject focusMesh;
    [SerializeField] private GameObject selectMesh;

    [SerializeField] private NeighbourType neighbourType; // Тип соседства
    [SerializeField] private Unit currentUnit; // Юнит, стоящий на клетке

    [SerializeField] private BattleController battleController;

    public Vector2Int BoardPosition { get; set; }
    //public event Action<Cell> OnPointerClickEvent;

     public NeighbourType NeighbourType 
    { 
        get => neighbourType; 
        set => neighbourType = value; 
    }
    
    public Unit CurrentUnit 
    { 
        get => currentUnit; 
        set => currentUnit = value; 
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        focusMesh.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        focusMesh.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log($"Cell clicked at position: {BoardPosition}");
        if (battleController != null)
            battleController.ProcessClick(this);

        //OnPointerClickEvent?.Invoke(this);
    }

    public void SetSelect(Material material)
    {
        selectMesh.SetActive(true);
        var renderer = selectMesh.GetComponent<MeshRenderer>();
        if (renderer != null)
        {
            renderer.material = material;
        }
    }

    public void ResetSelect()
    {
        selectMesh.SetActive(false);
    }
}
