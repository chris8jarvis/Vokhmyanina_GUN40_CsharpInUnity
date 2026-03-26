using UnityEngine;
using System.Collections.Generic;
using Units;

public static class MoveValidator
{

    // Диагональные направления: (dx, dy)
    private static readonly Vector2Int[] Diagonals =
    {
        new Vector2Int( 1,  1),
        new Vector2Int(-1,  1),
        new Vector2Int( 1, -1),
        new Vector2Int(-1, -1),
    };

    // Публичное API
    /// Возвращает доступные ходы. При наличии атак – только атаки.
    /// Заполняет attackTargets: destination -> жертва.
    public static List<Cell> GetAvailableMoves(Unit unit, Battlefield battlefield,
        out Dictionary<Cell, Unit> attackTargets)
    {
        attackTargets = new Dictionary<Cell, Unit>();

        if (unit == null || unit.CurrentCell == null) return new List<Cell>();

        var attackMoves = new List<Cell>();
        CollectAttacks(unit, battlefield, attackMoves, attackTargets,
            excludeCell: null);

        if (attackMoves.Count > 0) return attackMoves;

        // Нет атак – обычные ходы
        var normalMoves = new List<Cell>();
        CollectNormalMoves(unit, battlefield, normalMoves);
        return normalMoves;
    }

    public static bool HasAnyAttack(List<Unit> units, Battlefield battlefield)
    {
        foreach (var unit in units)
        {
            var attacks = new List<Cell>();
            var targets = new Dictionary<Cell, Unit>();
            CollectAttacks(unit, battlefield, attacks, targets, excludeCell: null);
            if (attacks.Count > 0) return true;
        }
        return false;
    }

    // Атаки конкретного юнита (с возможностью игнорировать уже убитую клетку).
    public static List<Cell> GetAttackMoves(Unit unit, Battlefield battlefield,
        Cell excludeCell, out Dictionary<Cell, Unit> attackTargets)
    {
        attackTargets = new Dictionary<Cell, Unit>();
        var attacks = new List<Cell>();
        CollectAttacks(unit, battlefield, attacks, attackTargets, excludeCell);
        return attacks;
    }

    private static void CollectNormalMoves(Unit unit, Battlefield battlefield,
        List<Cell> result)
    {
        Vector2Int pos = unit.CurrentCell.BoardPosition;

        if (unit.IsKing)
        {
            // Дамка – по всем диагоналям на любую длину
            foreach (var dir in Diagonals)
            {
                int nx = pos.x + dir.x;
                int ny = pos.y + dir.y;
                while (nx >= 0 && nx <= 7 && ny >= 0 && ny <= 7)
                {
                    Cell cell = battlefield.GetCellAtPosition(nx, ny);
                    if (cell == null) break;
                    if (cell.CurrentUnit != null) break; // клетка занята – стоп
                    result.Add(cell);
                    nx += dir.x;
                    ny += dir.y;
                }
            }
        }
        else
        {
            // Обычная шашка – только вперёд
            int dy = (unit.Player == Player.White) ? 1 : -1;
            TryAddNormal(pos.x - 1, pos.y + dy, battlefield, result);
            TryAddNormal(pos.x + 1, pos.y + dy, battlefield, result);
        }
    }

     private static void TryAddNormal(int x, int y, Battlefield battlefield, List<Cell> result)
    {
        if (x < 0 || x > 7 || y < 0 || y > 7) return;
        Cell cell = battlefield.GetCellAtPosition(x, y);
        if (cell != null && cell.CurrentUnit == null)
            result.Add(cell);
    }

    private static void CollectAttacks(Unit unit, Battlefield battlefield,
        List<Cell> attackMoves, Dictionary<Cell, Unit> attackTargets, Cell excludeCell)
    {
        Vector2Int pos = unit.CurrentCell.BoardPosition;

        if (unit.IsKing)
        {
            CollectKingAttacks(unit, pos, battlefield, attackMoves, attackTargets, excludeCell);
        }
        else
        {
            CollectManAttacks(unit, pos, battlefield, attackMoves, attackTargets, excludeCell);
        }
    }

    private static void CollectManAttacks(Unit unit, Vector2Int pos,
        Battlefield battlefield, List<Cell> attackMoves,
        Dictionary<Cell, Unit> attackTargets, Cell excludeCell)
    {
        foreach (var dir in Diagonals)
        {
            int ex = pos.x + dir.x;
            int ey = pos.y + dir.y;
            if (ex < 0 || ex > 7 || ey < 0 || ey > 7) continue;

            Cell enemyCell = battlefield.GetCellAtPosition(ex, ey);
            if (enemyCell == null) continue;
            if (enemyCell == excludeCell) continue; // уже убитая в этом ходу
            if (enemyCell.CurrentUnit == null) continue;
            if (enemyCell.CurrentUnit.Player == unit.Player) continue;

            // Клетка за врагом
            int lx = ex + dir.x;
            int ly = ey + dir.y;
            if (lx < 0 || lx > 7 || ly < 0 || ly > 7) continue;

            Cell landCell = battlefield.GetCellAtPosition(lx, ly);
            if (landCell == null) continue;
            if (landCell.CurrentUnit != null) continue; // занята

            attackMoves.Add(landCell);
            attackTargets[landCell] = enemyCell.CurrentUnit;
        }
    }
    
    private static void CollectKingAttacks(Unit unit, Vector2Int pos,
        Battlefield battlefield, List<Cell> attackMoves,
        Dictionary<Cell, Unit> attackTargets, Cell excludeCell)
    {
        foreach (var dir in Diagonals)
        {
            Unit foundEnemy = null;
            Cell foundEnemyCell = null;
            int nx = pos.x + dir.x;
            int ny = pos.y + dir.y;

            while (nx >= 0 && nx <= 7 && ny >= 0 && ny <= 7)
            {
                Cell cell = battlefield.GetCellAtPosition(nx, ny);
                if (cell == null) break;

                if (foundEnemy == null)
                {
                    if (cell.CurrentUnit != null)
                    {
                        if (cell == excludeCell)
                        {
                            // пропускаем уже убитую клетку
                            nx += dir.x; ny += dir.y;
                            continue;
                        }
                        if (cell.CurrentUnit.Player == unit.Player) break; // своя – стоп
                        // Нашли врага, теперь ищем свободные клетки за ним
                        foundEnemy = cell.CurrentUnit;
                        foundEnemyCell = cell;
                    }
                }
                else
                {
                    // После врага: добавляем все свободные клетки
                    if (cell.CurrentUnit != null) break; // другая фишка – стоп
                    attackMoves.Add(cell);
                    attackTargets[cell] = foundEnemy;
                }

                nx += dir.x;
                ny += dir.y;
            }
        }
    }
}
