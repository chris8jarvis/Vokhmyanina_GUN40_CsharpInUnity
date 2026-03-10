using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Units;

public class Battlefield : MonoBehaviour
{
    public event System.Action<Cell> OnCellClicked; // Событие для клика по клетке
    
    private List<Cell> allCells = new List<Cell>();
    private List<Unit> allUnits = new List<Unit>();
    
    private void Start()
    {
        FindAllCells();
        FindAllUnits();
        SetupCellNeighbours();
        SetupCellClickEvents();
        SetupUnitCellRelations();
    }
    
    private void FindAllCells()
    {
        allCells = FindObjectsOfType<Cell>().ToList();
        Debug.Log($"Found {allCells.Count} cells on scene");
    }
    
    private void FindAllUnits()
    {
        allUnits = FindObjectsOfType<Unit>().ToList();
        Debug.Log($"Found {allUnits.Count} units on scene");
    }
    
    private void SetupCellNeighbours()
    {
        foreach (Cell cell in allCells)
        {
            NeighbourType neighbours = CheckNeighbours(cell);
            cell.NeighbourType = neighbours;
            Debug.Log($"Cell {cell.name} neighbours: {neighbours}");
        }
    }
    
    // Проверка соседей для конкретной клетки
    private NeighbourType CheckNeighbours(Cell cell)
    {
        NeighbourType result = NeighbourType.None;
        
        Vector3 cellPos = cell.transform.position;
        float checkDistance = 1.1f; 
        
        if (HasCellAtPosition(cellPos + Vector3.left * checkDistance))
            result |= NeighbourType.Left;
            
        if (HasCellAtPosition(cellPos + Vector3.right * checkDistance))
            result |= NeighbourType.Right;
            
        if (HasCellAtPosition(cellPos + Vector3.forward * checkDistance))
            result |= NeighbourType.Top;
            
        if (HasCellAtPosition(cellPos + Vector3.back * checkDistance))
            result |= NeighbourType.Bottom;
            
        return result;
    }
    
    
    private bool HasCellAtPosition(Vector3 position)
    {
        Collider[] colliders = Physics.OverlapSphere(position, 0.1f);
        foreach (var collider in colliders)
        {
            if (collider.GetComponent<Cell>() != null)
                return true;
        }
        return false;
    }
    
    
    private void SetupCellClickEvents()
    {
        // foreach (Cell cell in allCells)
        // {
        //     cell.OnPointerClickEvent += OnCellClickedHandler;
        // }
    }
    
    private void OnCellClickedHandler(Cell cell)
    {
        Debug.Log($"Cell clicked: {cell.name}");
        OnCellClicked?.Invoke(cell);
    }
    
    
    private void SetupUnitCellRelations()
    {
        foreach (Unit unit in allUnits)
        {
            Cell cellUnderUnit = FindCellUnderUnit(unit);
            
            if (cellUnderUnit != null)
            {
                unit.CurrentCell = cellUnderUnit;
                cellUnderUnit.CurrentUnit = unit;
                Debug.Log($"Unit {unit.name} stands on cell {cellUnderUnit.name}");
            }
            else
            {
                Debug.LogWarning($"Unit {unit.name} is not on any cell!");
            }
        }
    }
    
    private Cell FindCellUnderUnit(Unit unit)
    {
        Vector3 rayStart = unit.transform.position + Vector3.up * 0.5f;
        RaycastHit[] hits = Physics.RaycastAll(rayStart, Vector3.down, 2f);
        
        foreach (var hit in hits)
        {
            if (hit.collider.gameObject == unit.gameObject) continue;
            
            Cell cell = hit.collider.GetComponent<Cell>();
            if (cell != null)
                return cell;
        }
        
        return null;
    }

    public Cell GetCellAtPosition(int x, int y)
    {
        foreach (Cell cell in allCells)
        {
            if (cell.BoardPosition.x == x && cell.BoardPosition.y == y)
            return cell;
        }
        return null;
    }

}