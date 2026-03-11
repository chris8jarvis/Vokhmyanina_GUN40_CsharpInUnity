using UnityEngine;
using System.Collections.Generic;
using Units;

public static class MoveValidator
{
    public static List<Cell> GetAvailableMoves(Unit unit, Battlefield battlefield)
    {
        List<Cell> availableMoves = new List<Cell>();
        //List<Cell> attackMoves = new List<Cell>();  //для атаки
        
        if (unit.CurrentCell == null) return availableMoves;
        
        int direction = (unit.Player == Player.White) ? 1 : -1;
        
        Vector2Int currentPos = unit.CurrentCell.BoardPosition;
        
        CheckDiagonal(currentPos.x - 1, currentPos.y + direction, unit, battlefield, availableMoves); //добавить attackMoves
        CheckDiagonal(currentPos.x + 1, currentPos.y + direction, unit, battlefield, availableMoves); //добавить attackMoves

        Debug.Log($"Unit at Board {unit.CurrentCell.BoardPosition}, World {unit.transform.position}");

        foreach (var move in availableMoves)
        {
            Debug.Log($"Available move: {move.BoardPosition}");
        }

        // if (attackMoves.Count > 0)
        // return attackMoves; //возвращаем ход с атакой если имеется
        
        return availableMoves;
    }
    
    private static void CheckDiagonal(int x, int y, Unit unit, Battlefield battlefield, List<Cell> availableMoves) // добавить List<Cell> attackMoves для атаки
    {
        if (x < 0 || x > 7 || y < 0 || y > 7) return;
        
        Cell targetCell = battlefield.GetCellAtPosition(x, y);
        if (targetCell == null) return;
        
        if (targetCell.CurrentUnit == null)
        {
            availableMoves.Add(targetCell);
        }
        //атака
        // else if (targetCell.CurrentUnit.Player != unit.Player)
        // {
        //     int attackX = x + (x - unit.CurrentCell.BoardPosition.x);
        //     int attackY = y + (y - unit.CurrentCell.BoardPosition.y);
        
        //     if (attackX >= 0 && attackX <= 7 && attackY >= 0 && attackY <= 7)
        //     {
        //         Cell attackCell = battlefield.GetCellAtPosition(attackX, attackY);
        //         if (attackCell != null && attackCell.CurrentUnit == null)
        //         {
        //             attackMoves.Add(attackCell);
        //         }
        //     }
        // }
    }
}
