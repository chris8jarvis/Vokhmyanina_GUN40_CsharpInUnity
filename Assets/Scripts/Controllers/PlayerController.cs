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
            if (unit == null)
            {
                Debug.LogError("ExecuteMove: unit is NULL!");
                isExecuting = false;
                return;
            }

            if (destination == null)
            {
                Debug.LogError($"ExecuteMove: destination is NULL for unit {unit.name}!");
                isExecuting = false;
                return;
            }

            if (destination.transform == null)
            {
                Debug.LogError($"ExecuteMove: destination.transform is NULL for cell {destination.name}!");
                isExecuting = false;
                return;
            }

            isExecuting = true;
            
            Debug.Log($"Visualizing move: {unit.name} to {destination.name}");

            if (battleController == null)
            {
                Debug.LogError("ExecuteMove: battleController is NULL! Assign in Inspector!");
                isExecuting = false;
                return;
            }
            
            Vector3 newPosition = destination.transform.position;
            newPosition.y += 0.5f;
            unit.transform.position = newPosition;
            
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
