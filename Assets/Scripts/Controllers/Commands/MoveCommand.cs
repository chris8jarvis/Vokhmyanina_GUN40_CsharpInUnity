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
                var playerController = GameObject.FindObjectOfType<Controllers.PlayerController>();
                playerController.ExecuteMove(selectedUnit, destination);
            }
            else
            {
                Debug.Log("Cell occupied");
                var battleController = GameObject.FindObjectOfType<Controllers.BattleController>();
                battleController.CancelAction();
            }
        }
    }
}
