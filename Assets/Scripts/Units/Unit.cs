using UnityEngine;
using UnityEngine.EventSystems;
using Controllers;
using System;

namespace Units
{
    public class Unit : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [SerializeField] private float moveSpeed = 5f;

        [SerializeField] private Player player; // Команда юнита

        [SerializeField] private Controllers.BattleController battleController;

        //[SerializeField] private BattleController battleController;
        
        private Cell currentCell;
        private bool isMoving = false;
        private Vector3 targetPosition;
        private Cell targetCell;
        
        public event Action OnMoveEndCallback;

        public Cell CurrentCell 
        { 
            get => currentCell; 
            set => currentCell = value; 
        }
        
        public Player Player //тут Team заменили на Player (public Player Player { get; set; })
        { 
            get => player; 
            set => player = value; 
        }
        
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
            //Debug.Log($"Unit clicked: {Owner} {Type} at {CurrentCell.BoardPosition}");
            if (battleController != null)
                battleController.ProcessClick(this);
            
            // if (currentCell != null)
            // {
            //     currentCell.OnPointerClick(eventData);
            // }
        }
        
        public void Move(Cell cell)
        {
            if (isMoving) return;
            
            targetCell = cell;
            targetPosition = cell.transform.position + Vector3.up;
            isMoving = true;
        }
    }
}