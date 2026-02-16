using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class Unit : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private float moveSpeed = 5f;
    private Cell currentCell;
    private bool isMoving = false;
    private Vector3 targetPosition;
    private Cell targetCell;
    
    public event Action OnMoveEndCallback;
    
    private void Start()
    {
        FindCurrentCell();
    }
    
    private void Update()
    {
        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(
                transform.position, 
                targetPosition, 
                moveSpeed * Time.deltaTime
            );
            
            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                isMoving = false;
                transform.position = targetPosition;
                currentCell = targetCell;
                OnMoveEndCallback?.Invoke();
            }
        }
    }
    
    private void FindCurrentCell()
    {
        float rayLength = 3f;
        Vector3 rayStart = transform.position + Vector3.up * 0.5f;
        
        RaycastHit[] hits = Physics.RaycastAll(rayStart, Vector3.down, rayLength);
        
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.gameObject == gameObject) continue;
            
            Cell cell = hit.collider.GetComponent<Cell>();
            if (cell != null)
            {
                currentCell = cell;
                return;
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (currentCell != null)
        {
            currentCell.OnPointerEnter(eventData);
        }
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        if (currentCell != null)
        {
            currentCell.OnPointerExit(eventData);
        }
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (currentCell != null)
        {
            currentCell.OnPointerClick(eventData);
        }
    }
    
    public void Move(Cell cell)
    {
        if (isMoving) return;
        
        targetCell = cell;
        targetPosition = cell.transform.position + Vector3.up;
        isMoving = true;
    }
}