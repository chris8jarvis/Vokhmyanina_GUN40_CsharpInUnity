using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Units;
using Zenject;

public class Battlefield : MonoBehaviour
{
    private CellPaletteSettings palette;
    public event System.Action<Cell> OnCellClicked;
    
    private List<Cell> allCells = new List<Cell>();
    private List<Unit> allUnits = new List<Unit>();
    private Dictionary<Vector2Int, Cell> cellMap = new Dictionary<Vector2Int, Cell>();

    private List<Cell> highlightedCells = new List<Cell>();

    private Unit highlightedUnit;

    [Inject]
    public void Construct(CellPaletteSettings paletteSettings)
    {
        palette = paletteSettings;
    }
    
    private void Start()
    {
        FindAllCells();
        AssignBoardPositions();
        BuildCellMap();
        FindAllUnits();
        SetupCellNeighbours();
        SetupUnitCellRelations();
    }

    //подсветка
    public void HighlightMoves(List<Cell> moves, Dictionary<Cell, Unit> attackTargets)
    {
        foreach (var cell in moves)
        {
            bool isAttack = attackTargets != null && attackTargets.ContainsKey(cell);
            Material mat = isAttack ? palette.attackMaterial : palette.availableMaterial;
            cell.SetSelect(mat);
            highlightedCells.Add(cell);
        }
    }
    
    /// Подсвечивает выбранную фишку: клетку и юнита на ней.
    public void HighlightSelected(Cell cell)
    {
        cell.SetSelect(palette.selectedMaterial);
        if (!highlightedCells.Contains(cell))
            highlightedCells.Add(cell);

        // Подсвечиваем самого юнита
        if (cell.CurrentUnit != null)
        {
            var mat = palette.unitSelectedMaterial != null
                ? palette.unitSelectedMaterial
                : palette.selectedMaterial;
            cell.CurrentUnit.SetHighlight(mat);
            highlightedUnit = cell.CurrentUnit;
        }
    }

    /// Убирает всю подсветку.
    public void ClearHighlights()
    {
        foreach (var cell in highlightedCells)
            cell.ResetSelect();
        highlightedCells.Clear();

        // Сбрасываем подсветку юнита
        if (highlightedUnit != null)
        {
            highlightedUnit.ResetHighlight();
            highlightedUnit = null;
        }
    }

    // Юниты
    public List<Unit> GetUnitsOfPlayer(Player player)
    {
        return allUnits.Where(u => u != null && u.Player == player).ToList();
    }

    public void RemoveUnit(Unit unit)
    {
        allUnits.Remove(unit);
    }


    private void FindAllCells()
    {
        allCells = FindObjectsOfType<Cell>().ToList();
    }

    private void AssignBoardPositions()
    {
        var sortedCells = allCells
            .OrderBy(c => c.transform.position.z)
            .ThenBy(c => c.transform.position.x)
            .ToList();
    
        int index = 0;
        for (int y = 0; y < 8; y++)
        {
            for (int x = 0; x < 8; x++)
            {
                if (index < sortedCells.Count)
                {
                    sortedCells[index].BoardPosition = new Vector2Int(x, y);
                    index++;
                }
            }
        }
    }
    
    private void BuildCellMap()
    {
        cellMap.Clear();
        foreach (var cell in allCells)
            cellMap[cell.BoardPosition] = cell;
    }

    private void FindAllUnits()
    {
        allUnits = FindObjectsOfType<Unit>().ToList();
    }
    
    private void SetupCellNeighbours()
    {
        foreach (Cell cell in allCells)
        {
            cell.NeighbourType = CheckNeighbours(cell);
        }
    }
    
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

    private void SetupUnitCellRelations()
    {
        foreach (Unit unit in allUnits)
        {
            Cell cellUnderUnit = FindCellUnderUnit(unit);
            if (cellUnderUnit != null)
            {
                unit.CurrentCell = cellUnderUnit;
                cellUnderUnit.CurrentUnit = unit;
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
        cellMap.TryGetValue(new Vector2Int(x, y), out Cell cell);
        return cell;
    }

}