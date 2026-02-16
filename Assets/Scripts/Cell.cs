using UnityEngine.EventSystems;
using UnityEngine;
using System;

public class Cell : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private GameObject focusMesh;
    [SerializeField] private GameObject selectMesh;
    
    public event Action<Cell> OnPointerClickEvent;
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
        OnPointerClickEvent?.Invoke(this);
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
