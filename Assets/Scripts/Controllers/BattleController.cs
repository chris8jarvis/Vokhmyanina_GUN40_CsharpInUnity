using UnityEngine;
using Commands;
using Units;

namespace Controllers
{
    public class BattleController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private PlayerController playerController;
        
        [Header("State")]
        public Player currentPlayer = Player.White;
        public GameState currentState = GameState.SelectUnit;
        
        private IGameplayCommand currentCommand;
        private Unit selectedUnit;
        
        private void Start()
        {
            // Начало игры - белые ходят первыми
            currentPlayer = Player.White;
            currentState = GameState.SelectUnit;
            Debug.Log($"Game started. {currentPlayer} turn");
        }
        
        
        public void ProcessClick(Cell cell)
        {
            if (currentCommand != null)
            {
                currentCommand.Interact(cell);
                
                // После выполнения команды сбрасываем её
                if (currentState == GameState.SelectDestination)
                {
                    // TODO: Переключение игрока после хода
                }
            }
        }
        
        public void ProcessClick(Unit unit)
        {
            if (currentState == GameState.SelectUnit && unit.Player == currentPlayer)
            {
                selectedUnit = unit;
                currentCommand = new SelectCommand();
                currentCommand.Interact(unit.CurrentCell);
                
                currentState = GameState.SelectDestination;
                currentCommand = new MoveCommand(selectedUnit);
            }
        }
        
        public void CancelAction()
        {
            // Сброс выбора (по ESC)
            selectedUnit = null;
            currentCommand = null;
            currentState = GameState.SelectUnit;
            Debug.Log("Action cancelled");
        }
    }
}
