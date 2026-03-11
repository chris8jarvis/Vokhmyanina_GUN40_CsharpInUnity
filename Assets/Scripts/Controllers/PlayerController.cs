using UnityEngine;
using Units;

namespace Controllers
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private BattleController battleController;
        
        private bool isExecuting = false;
        
        public bool CanInteract => !isExecuting;
        
        public async void ExecuteMove(Unit unit, Cell destination)
        {
             if (unit == null || destination == null)
            {
                Debug.LogError("ExecuteMove: unit or destination is NULL!");
                isExecuting = false;
                return;
            }

            isExecuting = true;
            
            Debug.Log($"Visualizing move: {unit.name} to {destination.name}");

            
            unit.Teleport(destination);
            
            if (unit.CurrentCell != null)
                unit.CurrentCell.CurrentUnit = null;
                
            unit.CurrentCell = destination;
            destination.CurrentUnit = unit;

            if (!unit.IsKing)
            {
                if (unit.Player == Player.White && destination.BoardPosition.y == 7)
                {
                    unit.BecomeKing();
                    Debug.Log("White became KING");
                }
                else if (unit.Player == Player.Black && destination.BoardPosition.y == 0)
                {
                    unit.BecomeKing();
                    Debug.Log("Black became KING");
                }
            }
            
            await System.Threading.Tasks.Task.Delay(300);
            
            isExecuting = false;
            
            battleController.currentPlayer = battleController.currentPlayer == Player.White ? 
                Player.Black : Player.White;
            battleController.currentState = GameState.SelectUnit;
            
            Debug.Log($"{battleController.currentPlayer} turn");
        }
    }
}
