using Controllers;
using System.Collections.Generic;
using UnityEngine;
using Units;

namespace Commands
{
    public class MoveCommand : IGameplayCommand
    {
        private readonly Battlefield m_battlefield;
        private readonly PlayerController m_playerController;

        // Текущие доступные ходы и цели для атаки
        public List<Cell> AvailableMoves { get; private set; } = new();
        public Dictionary<Cell, Unit> AttackTargets { get; private set; } = new();
        
        public CommandType Type => CommandType.Move;


        public MoveCommand(PlayerController playerController, Battlefield battlefield)
        {
            m_playerController = playerController;
            m_battlefield = battlefield;
        }

        /// Вычисляет и кэширует доступные ходы для юнита.
        /// Вызывается из BattleController перед переходом в SelectDestination.
        public void PrepareForUnit(Unit unit)
        {
            AvailableMoves = MoveValidator.GetAvailableMoves(unit, m_battlefield, out var targets);
            AttackTargets = targets;
        }
        
        public bool TryInteract(Cell destination, Unit selectedUnit)
        {
            if (!AvailableMoves.Contains(destination))
            {
                Debug.Log("Invalid move!");
                return false;
            }
            AttackTargets.TryGetValue(destination, out Unit enemyToKill);

            m_battlefield.ClearHighlights();
            m_playerController.ExecuteMove(selectedUnit, destination, enemyToKill, m_battlefield);
            return true;
        }
    }
}
