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
            var battlefield = GameObject.FindObjectOfType<Battlefield>();
            var availableMoves = MoveValidator.GetAvailableMoves(selectedUnit, battlefield);

            if (availableMoves.Contains(destination))
            {
                var playerController = GameObject.FindObjectOfType<Controllers.PlayerController>();
                playerController.ExecuteMove(selectedUnit, destination);
            }
            else
            {
                Debug.Log("Invalid move!");
                var battleController = GameObject.FindObjectOfType<Controllers.BattleController>();
                battleController.CancelAction();
            }

            // if (destination.CurrentUnit == null)
            // {
            //     var playerController = GameObject.FindObjectOfType<Controllers.PlayerController>();
            //     playerController.ExecuteMove(selectedUnit, destination);
            // }
            // else
            // {
            //     Debug.Log("Cell occupied");
            //     var battleController = GameObject.FindObjectOfType<Controllers.BattleController>();
            //     battleController.CancelAction();
            // }
        }
    }
}
