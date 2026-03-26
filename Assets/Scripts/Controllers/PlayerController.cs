using System;
using System.Collections.Generic;
using UnityEngine;
using Units;
using Zenject;

namespace Controllers
{
    public class PlayerController : MonoBehaviour
    {
        private GameStateSystem m_stateSystem;
        private bool m_isExecuting;
        public bool CanInteract => !m_isExecuting;

        public event Action OnTurnEnded;

        public event Action<Unit, List<Cell>, Dictionary<Cell, Unit>, Battlefield> OnAttackChainAvailable;

        [Inject]
        public void Construct(GameStateSystem stateSystem)
        {
            m_stateSystem = stateSystem;
        }
        
        public async void ExecuteMove(Unit unit, Cell destination, Unit enemyToKill, Battlefield battlefield)
        {
             if (unit == null || destination == null)
            {
                Debug.LogError("ExecuteMove: unit or destination is NULL!");
                m_isExecuting = false;
                return;
            }

            m_isExecuting = true;

            Cell killedCell = enemyToKill != null ? enemyToKill.CurrentCell : null;
            
            bool isKilled = false;

            unit.Teleport(destination);

            // Убираем убитого
            if (enemyToKill != null)
            {
                // Освобождаем клетку убитого (если не была переписана Teleport'ом)
                if (killedCell != null && killedCell.CurrentUnit == enemyToKill)
                    killedCell.CurrentUnit = null;
                battlefield.RemoveUnit(enemyToKill);
                Destroy(enemyToKill.gameObject);
                isKilled = true;
                Debug.Log("Enemy killed!");
            }

            // Проверяем дамку
            bool becameKing = false;
            if (!unit.IsKing)
            {
                if ((unit.Player == Player.White && destination.BoardPosition.y == 7) ||
                    (unit.Player == Player.Black && destination.BoardPosition.y == 0))
                {
                    unit.BecomeKing();
                    becameKing = true;
                    Debug.Log($"{unit.Player} became KING");
                }
            }

            await System.Threading.Tasks.Task.Delay(300);

            m_isExecuting = false;

            // Если была атака — проверяем цепочку
            if (isKilled && !becameKing)
            {
                // Враг уже уничтожен и клетка реально пуста — excludeCell не нужен
                var chainAttacks = MoveValidator.GetAttackMoves(unit, battlefield,
                    null, out var chainTargets);

                if (chainAttacks.Count > 0)
                {
                    OnAttackChainAvailable?.Invoke(unit, chainAttacks, chainTargets, battlefield);
                    return;
                }
            }

             m_stateSystem.currentPlayer = m_stateSystem.currentPlayer == Player.White
                ? Player.Black : Player.White;
            m_stateSystem.currentState = GameState.SelectUnit;

            OnTurnEnded?.Invoke();
            Debug.Log($"{m_stateSystem.currentPlayer} turn");
        }
    }
}
