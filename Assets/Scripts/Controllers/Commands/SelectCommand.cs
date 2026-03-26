using System.Collections.Generic;
using UnityEngine;
using Units;

namespace Commands
{
    public class SelectCommand : IGameplayCommand
    {
        private readonly Battlefield m_battlefield;
        
        public CommandType Type => CommandType.Select;
        
        public SelectCommand(Battlefield battlefield)
        {
            m_battlefield = battlefield;
        }

        public bool TryInteract(Cell cell, Unit selectedUnit)
        {
            if (selectedUnit == null) return false;

            var moves = MoveValidator.GetAvailableMoves(selectedUnit, m_battlefield,
                out Dictionary<Cell, Unit> attackTargets);

            m_battlefield.HighlightSelected(selectedUnit.CurrentCell);
            m_battlefield.HighlightMoves(moves, attackTargets);

            Debug.Log($"Selected {selectedUnit.name}. Moves: {moves.Count}");
            return true;
        }
    }
}
