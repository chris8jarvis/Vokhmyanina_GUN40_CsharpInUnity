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
            if (destination.CurrentUnit == null)
            {
            Debug.Log($"Moving {selectedUnit.Player} to {destination.BoardPosition}");
            // TODO: Добавить проверку правил шашек
            // TODO: Здесь будем вызывать PlayerController для визуализации
            // PlayerController.ExecuteMove(selectedUnit, destination);
            }
             else
            {
                Debug.Log($"Cannot move to occupied cell!");
                
                // Возвращаемся к выбору фигуры
                var battleController = GameObject.FindObjectOfType<Controllers.BattleController>();
                battleController.CancelAction();
            }
        }
    }
}
