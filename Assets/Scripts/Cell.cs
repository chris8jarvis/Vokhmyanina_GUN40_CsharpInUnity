using UnityEngine.EventSystems;
using UnityEngine;
using Controllers;
using Zenject;
using Units;

public class Cell : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private GameObject focusMesh;
    [SerializeField] private GameObject selectMesh;

    [SerializeField] private NeighbourType neighbourType;
    [SerializeField] private Unit currentUnit;

    private BattleController m_battleController;

    [Inject]
    public void Construct(BattleController battleController)
    {
        m_battleController = battleController;
    }

    public Vector2Int BoardPosition { get; set; }

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
        if (m_battleController != null)
            m_battleController.ProcessClick(this);
    }

    public void SetSelect(Material material)
    {
        selectMesh.SetActive(true);
        var meshRenderer = selectMesh.GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            meshRenderer.material = material;
        }
    }

    public void ResetSelect()
    {
        selectMesh.SetActive(false);
    }
}
