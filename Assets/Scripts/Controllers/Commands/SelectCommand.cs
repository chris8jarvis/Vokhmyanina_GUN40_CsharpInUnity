using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Units;

namespace Commands
{
    public class SelectCommand : IGameplayCommand
    {
        private Unit selectedUnit;
        
        public void Interact(Cell cell)
        {
            if (cell.CurrentUnit != null)
            {
                selectedUnit = cell.CurrentUnit;
                //Debug.Log($"Selected {selectedUnit.Player} at {cell.BoardPosition}");

                var battlefield = GameObject.FindObjectOfType<Battlefield>();
                var moves = MoveValidator.GetAvailableMoves(selectedUnit, battlefield);
                Debug.Log($"Available moves: {moves.Count}");
            }
            else
            {
                Debug.Log("Empty cell clicked");
            }
        }
    }
}
