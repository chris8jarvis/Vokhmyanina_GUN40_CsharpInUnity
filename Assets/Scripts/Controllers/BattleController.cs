using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Commands;
using Units;
using Zenject;

namespace Controllers
{
    public sealed class BattleController : IInitializable, IDisposable
    {
        private readonly GameStateSystem m_stateSystem;
        private readonly InputActionAsset m_inputActions;
        private readonly Battlefield m_battlefield;
        private readonly SelectCommand m_selectCommand;
        private readonly MoveCommand m_moveCommand;
        private readonly PlayerController m_playerController;

        private InputActionMap m_uiActionMap;
        private InputAction m_cancelAction;
        private InputAction m_submitAction;

        private Unit m_currentUnit;

        private List<Unit> m_forcedAttackUnits = new();
        private List<Cell> m_chainMoves = new();
        private Dictionary<Cell, Unit> m_chainTargets = new();

        public BattleController(
            InputActionAsset inputActions,
            GameStateSystem stateSystem,
            Battlefield battlefield,
            SelectCommand selectCommand,
            MoveCommand moveCommand,
            PlayerController playerController)
        {
            m_inputActions = inputActions;
            m_stateSystem = stateSystem;
            m_battlefield = battlefield;
            m_selectCommand = selectCommand;
            m_moveCommand = moveCommand;
            m_playerController = playerController;
        }
        
        void IInitializable.Initialize()
        {
            m_uiActionMap = m_inputActions.FindActionMap("UI");
            m_cancelAction = m_uiActionMap?.FindAction("Cancel");
            m_submitAction = m_uiActionMap?.FindAction("Submit");

            if (m_cancelAction != null) { m_cancelAction.performed += OnCancel; m_cancelAction.Enable(); }
            if (m_submitAction != null) { m_submitAction.performed += OnSubmit; m_submitAction.Enable(); }

            // Подписываемся на события PlayerController здесь — после создания обоих объектов.
            // Это разрывает циклическую зависимость через Zenject.
            m_playerController.OnTurnEnded += OnTurnStarted;
            m_playerController.OnAttackChainAvailable += StartAttackChain;

            // Случайный первый ход
            m_stateSystem.currentPlayer = UnityEngine.Random.value > 0.5f ? Player.White : Player.Black;
            m_stateSystem.currentState = GameState.SelectUnit;

            Debug.Log($"Game started. {m_stateSystem.currentPlayer} goes first");
        }

        void IDisposable.Dispose()
        {
            m_cancelAction.performed -= OnCancel;
            m_submitAction.performed -= OnSubmit;

            m_playerController.OnTurnEnded -= OnTurnStarted;
            m_playerController.OnAttackChainAvailable -= StartAttackChain;

            m_uiActionMap?.Dispose();
        }

        // -------------------------------------------------------------------------
        // Входные события
        // -------------------------------------------------------------------------

        private void OnCancel(InputAction.CallbackContext ctx) 
            => CancelAction();
        
        private void OnSubmit(InputAction.CallbackContext ctx) 
            => Debug.Log("Submit pressed");

        public void CancelAction()
        {
            // В AttackChain выйти нельзя
            if (m_stateSystem.currentState == GameState.AttackChain) return;

            m_battlefield.ClearHighlights();
            m_currentUnit = null;
            m_stateSystem.currentState = GameState.SelectUnit;
            Debug.Log("Action cancelled");
        }

        // -------------------------------------------------------------------------
        // Клик по юниту
        // -------------------------------------------------------------------------

        public void ProcessClick(Unit unit)
        {
            if (m_stateSystem.currentState == GameState.AttackChain) return;
            if (m_stateSystem.currentState != GameState.SelectUnit) return;
            if (unit.Player != m_stateSystem.currentPlayer) return;

            // Проверяем обязательную атаку
            if (m_forcedAttackUnits.Count > 0 && !m_forcedAttackUnits.Contains(unit))
            {
                Debug.Log("Вы обязаны атаковать одной из подсвеченных фишек!");
                return;
            }

            SelectUnit(unit);
        }

        // -------------------------------------------------------------------------
        // Клик по клетке
        // -------------------------------------------------------------------------

        public void ProcessClick(Cell cell)
        {
            var state = m_stateSystem.currentState;

            if (state == GameState.SelectUnit)
            {
                // Клик по клетке с юнитом текущего игрока — выбираем юнита
                if (cell.CurrentUnit != null && cell.CurrentUnit.Player == m_stateSystem.currentPlayer)
                    ProcessClick(cell.CurrentUnit);
                return;
            }

            if (state == GameState.SelectDestination || state == GameState.AttackChain)
            {
                // Клик по своей фишке в SelectDestination — переназначаем выбор
                if (state == GameState.SelectDestination &&
                    cell.CurrentUnit != null &&
                    cell.CurrentUnit.Player == m_stateSystem.currentPlayer)
                {
                    // Если обязательная атака — разрешён выбор только из forcedAttackUnits
                    if (m_forcedAttackUnits.Count > 0 && !m_forcedAttackUnits.Contains(cell.CurrentUnit))
                    {
                        Debug.Log("Вы обязаны атаковать одной из подсвеченных фишек!");
                        return;
                    }
                    SelectUnit(cell.CurrentUnit);
                    return;
                }

                // Попытка сделать ход
                if (m_currentUnit == null) return;

                // В AttackChain — используем chainMoves
                if (state == GameState.AttackChain)
                {
                    if (!m_chainMoves.Contains(cell))
                    {
                        Debug.Log("В цепочке атак нужно бить врага!");
                        return;
                    }
                    // Подменяем MoveCommand данными цепочки
                    m_moveCommand.AvailableMoves.Clear();
                    m_moveCommand.AvailableMoves.AddRange(m_chainMoves);
                    m_moveCommand.AttackTargets.Clear();
                    foreach (var kv in m_chainTargets) m_moveCommand.AttackTargets[kv.Key] = kv.Value;
                }

                bool moved = m_moveCommand.TryInteract(cell, m_currentUnit);
                if (!moved && state == GameState.SelectDestination)
                    CancelAction();
            }
        }

        // -------------------------------------------------------------------------
        // Начало хода — вызывается через событие PlayerController.OnTurnEnded
        // -------------------------------------------------------------------------

        private void OnTurnStarted()
        {
            m_currentUnit = null;
            m_forcedAttackUnits.Clear();
            m_chainMoves.Clear();
            m_chainTargets.Clear();

            // Проверяем обязательные атаки для текущего игрока
            var myUnits = m_battlefield.GetUnitsOfPlayer(m_stateSystem.currentPlayer);
            if (MoveValidator.HasAnyAttack(myUnits, m_battlefield))
            {
                // Заполняем список фишек, которые могут атаковать
                foreach (var unit in myUnits)
                {
                    var attacks = MoveValidator.GetAttackMoves(unit, m_battlefield, null, out _);
                    if (attacks.Count > 0)
                        m_forcedAttackUnits.Add(unit);
                }
                Debug.Log($"Обязательная атака! Атакующих фишек: {m_forcedAttackUnits.Count}");
            }
        }

        // -------------------------------------------------------------------------
        // Цепочка атак — вызывается через событие PlayerController.OnAttackChainAvailable
        // -------------------------------------------------------------------------

        private void StartAttackChain(Unit unit, List<Cell> attacks,
            Dictionary<Cell, Unit> targets, Battlefield battlefield)
        {
            m_currentUnit = unit;
            m_chainMoves = attacks;
            m_chainTargets = targets;
            m_stateSystem.currentState = GameState.AttackChain;

            // Подсвечиваем цепочку
            m_battlefield.ClearHighlights();
            m_battlefield.HighlightSelected(unit.CurrentCell);
            m_battlefield.HighlightMoves(attacks, targets);

            Debug.Log($"Цепочка атак! Вариантов: {attacks.Count}");
        }

        // -------------------------------------------------------------------------
        // Вспомогательные методы
        // -------------------------------------------------------------------------

        private void SelectUnit(Unit unit)
        {
            m_battlefield.ClearHighlights();
            m_currentUnit = unit;

            // Готовим MoveCommand
            m_moveCommand.PrepareForUnit(unit);

            // Визуально выделяем
            m_selectCommand.TryInteract(unit.CurrentCell, unit);

            m_stateSystem.currentState = GameState.SelectDestination;
            Debug.Log($"Selected unit: {unit.name} ({unit.Player})");
        }
    }
}
