using UnityEngine;
using System.Collections.Generic;
using Units;

public static class MoveValidator
{

    private static Dictionary<Cell, Unit> attackTargets = new Dictionary<Cell, Unit>(); //хранение информации, кого атакует шашка
    public static List<Cell> GetAvailableMoves(Unit unit, Battlefield battlefield)
    {
        List<Cell> availableMoves = new List<Cell>();
        List<Cell> attackMoves = new List<Cell>();  //для атаки
        attackTargets.Clear();
        
        if (unit.CurrentCell == null) return availableMoves;
        
        Vector2Int currentPos = unit.CurrentCell.BoardPosition;

        CheckDiagonalForAttack(currentPos.x - 1, currentPos.y + 1, unit, battlefield, attackMoves);
        CheckDiagonalForAttack(currentPos.x + 1, currentPos.y + 1, unit, battlefield, attackMoves);
        CheckDiagonalForAttack(currentPos.x - 1, currentPos.y - 1, unit, battlefield, attackMoves);
        CheckDiagonalForAttack(currentPos.x + 1, currentPos.y - 1, unit, battlefield, attackMoves);
        

        //Debug.Log($"Unit at Board {unit.CurrentCell.BoardPosition}, World {unit.transform.position}");

         if (attackMoves.Count > 0)
        {
            foreach (var move in attackMoves)
            {
                Debug.Log($"Attack move: {move.BoardPosition}");
            }
            return attackMoves;
        }

        int direction = (unit.Player == Player.White) ? 1 : -1;
        CheckNormalMove(currentPos.x - 1, currentPos.y + direction, unit, battlefield, availableMoves);
        CheckNormalMove(currentPos.x + 1, currentPos.y + direction, unit, battlefield, availableMoves);
        
        foreach (var move in availableMoves)
        {
            Debug.Log($"Normal move: {move.BoardPosition}");
        }
        
        return availableMoves;
    }
    
    private static void CheckNormalMove(int x, int y, Unit unit, Battlefield battlefield, List<Cell> availableMoves)
    {
        if (x < 0 || x > 7 || y < 0 || y > 7) return;
        
        Cell targetCell = battlefield.GetCellAtPosition(x, y);
        if (targetCell == null) return;
        
        if (targetCell.CurrentUnit == null)
        {
            availableMoves.Add(targetCell);
        }
    }

     private static void CheckDiagonalForAttack(int x, int y, Unit unit, Battlefield battlefield, List<Cell> attackMoves)
    {
        if (x < 0 || x > 7 || y < 0 || y > 7) return;
        
        Cell targetCell = battlefield.GetCellAtPosition(x, y);
        if (targetCell == null) return;
        
        if (targetCell.CurrentUnit != null && targetCell.CurrentUnit.Player != unit.Player)
        {
            int attackX = x + (x - unit.CurrentCell.BoardPosition.x);
            int attackY = y + (y - unit.CurrentCell.BoardPosition.y);
            
            if (attackX >= 0 && attackX <= 7 && attackY >= 0 && attackY <= 7)
            {
                Cell attackCell = battlefield.GetCellAtPosition(attackX, attackY);
                if (attackCell != null && attackCell.CurrentUnit == null)
                {
                    attackMoves.Add(attackCell);
                    attackTargets[attackCell] = targetCell.CurrentUnit;
                    Debug.Log($"Found attack: from {unit.CurrentCell.BoardPosition} to {attackCell.BoardPosition}, killing at ({x},{y})");
                }
            }
        }
    }
     public static Unit GetAttackTarget(Cell destination)
    {
        if (attackTargets.ContainsKey(destination))
            return attackTargets[destination];
            return null;
    }
}
