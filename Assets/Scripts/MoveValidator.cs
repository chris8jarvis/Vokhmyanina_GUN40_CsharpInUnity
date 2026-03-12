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
        attackTargets.Clear(); //очистка старых данных
        
        if (unit.CurrentCell == null) return availableMoves;
        
        int direction = (unit.Player == Player.White) ? 1 : -1;
        Vector2Int currentPos = unit.CurrentCell.BoardPosition;
        
        CheckDiagonal(currentPos.x - 1, currentPos.y + direction, unit, battlefield, availableMoves, attackMoves); //attackMoves для атаки
        CheckDiagonal(currentPos.x + 1, currentPos.y + direction, unit, battlefield, availableMoves, attackMoves); //attackMoves для атаки

        Debug.Log($"Unit at Board {unit.CurrentCell.BoardPosition}, World {unit.transform.position}");

        foreach (var move in availableMoves)
        {
            Debug.Log($"Available move: {move.BoardPosition}");
        }
        //возвращаем ход с атакой если имеется
        if (attackMoves.Count > 0)
        {
            return attackMoves;
        } 
        
        return availableMoves;
    }
    
    private static void CheckDiagonal(int x, int y, Unit unit, Battlefield battlefield, List<Cell> availableMoves, List<Cell> attackMoves) // List<Cell> attackMoves для атаки
    {
        if (x < 0 || x > 7 || y < 0 || y > 7) return;
        
        Cell targetCell = battlefield.GetCellAtPosition(x, y);
        if (targetCell == null) return;
        
        if (targetCell.CurrentUnit == null)
        {
            availableMoves.Add(targetCell);
        }
        //атака
        else if (targetCell.CurrentUnit.Player != unit.Player)
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
                 }
             }
         }
    }
    //получение цели атаки
     public static Unit GetAttackTarget(Cell destination)
    {
        if (attackTargets.ContainsKey(destination))
            return attackTargets[destination];
            return null;
    }
}
