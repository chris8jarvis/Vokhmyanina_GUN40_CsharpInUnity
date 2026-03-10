using UnityEngine;
using System.Collections.Generic;
using Units;

public static class MoveValidator
{
    public static List<Cell> GetAvailableMoves(Unit unit, Battlefield battlefield)
    {
        List<Cell> availableMoves = new List<Cell>();
        
        if (unit.CurrentCell == null) return availableMoves;
        
        int direction = (unit.Player == Player.White) ? 1 : -1;
        
        Vector2Int currentPos = unit.CurrentCell.BoardPosition;
        
        CheckDiagonal(currentPos.x - 1, currentPos.y + direction, unit, battlefield, availableMoves);
        CheckDiagonal(currentPos.x + 1, currentPos.y + direction, unit, battlefield, availableMoves);
        
        return availableMoves;
    }
    
    private static void CheckDiagonal(int x, int y, Unit unit, Battlefield battlefield, List<Cell> availableMoves)
    {
        if (x < 0 || x > 7 || y < 0 || y > 7) return;
        
        Cell targetCell = battlefield.GetCellAtPosition(x, y);
        if (targetCell == null) return;
        
        if (targetCell.CurrentUnit == null)
        {
            availableMoves.Add(targetCell);
        }
    }
}
