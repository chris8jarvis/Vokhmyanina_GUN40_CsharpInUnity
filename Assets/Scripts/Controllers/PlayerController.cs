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
            isExecuting = true;
            
            // TODO: Добавить анимацию перемещения
            Debug.Log($"Visualizing move: {unit.name} to {destination.name}");
            
            // Перемещаем фигуру
            unit.transform.position = destination.transform.position;
            
            // Обновляем ссылки
            if (unit.CurrentCell != null)
                unit.CurrentCell.CurrentUnit = null;
                
            unit.CurrentCell = destination;
            destination.CurrentUnit = unit;
            
            // Имитация времени на визуализацию
            await System.Threading.Tasks.Task.Delay(300);
            
            isExecuting = false;
            
            // Переключаем игрока
            battleController.currentPlayer = battleController.currentPlayer == Player.White ? 
                Player.Black : Player.White;
            battleController.currentState = GameState.SelectUnit;
            
            Debug.Log($"{battleController.currentPlayer} turn");
        }
    }
}
