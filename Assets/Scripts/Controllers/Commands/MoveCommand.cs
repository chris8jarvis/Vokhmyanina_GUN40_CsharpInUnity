using UnityEngine;
using Units;

namespace Commands
{
    public class MoveCommand : IGameplayCommand
    {
        private Unit selectedUnit;
        
        public MoveCommand(Unit unit)
        {
            selectedUnit = unit;
        }
        
        public void Interact(Cell destination)
        {
            Debug.Log($"Moving {selectedUnit.Player} to {destination.BoardPosition}");
            // TODO: Добавить проверку правил шашек
        }
    }
}
