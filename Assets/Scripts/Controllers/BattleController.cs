using UnityEngine;
using UnityEngine.InputSystem;
using Commands;
using Units;

namespace Controllers
{
    public class BattleController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private PlayerController playerController;
        [SerializeField] private InputActionAsset inputActions;
        
        [Header("State")]
        public Player currentPlayer = Player.White;
        public GameState currentState = GameState.SelectUnit;
        
        private IGameplayCommand currentCommand;
        private Unit selectedUnit;
        private InputActionMap uiActionMap;
        private InputAction cancelAction;
        private InputAction submitAction;

        private void Awake()
        {
            uiActionMap = inputActions.FindActionMap("UI");
            cancelAction = uiActionMap.FindAction("Cancel");
            submitAction = uiActionMap.FindAction("Submit");
        }

        private void OnEnable()
        {
            cancelAction.performed += OnCancel;
            submitAction.performed += OnSubmit;
            cancelAction.Enable();
            submitAction.Enable();
        }

        private void OnDisable()
        {
            cancelAction.performed -= OnCancel;
            submitAction.performed -= OnSubmit;
        }
        
        private void Start()
        {
            // Начало игры - белые ходят первыми
            currentPlayer = Player.White;
            currentState = GameState.SelectUnit;
            Debug.Log($"Game started. {currentPlayer} turn");
        }

        private void OnCancel(InputAction.CallbackContext context)
        {
            CancelAction();
        }

        private void OnSubmit(InputAction.CallbackContext context)
        {
            Debug.Log("Submit pressed");
        }

        public void CancelAction()
        {
            selectedUnit = null;
            currentCommand = null;
            currentState = GameState.SelectUnit;
            Debug.Log("Action cancelled");
        }
        
        
        public void ProcessClick(Cell cell)
        {
            if (currentCommand != null)
            {
                currentCommand.Interact(cell);
                
                if (currentState == GameState.SelectDestination)
                {
                    currentState = GameState.SelectUnit;
                    currentCommand = null;
                    selectedUnit = null;
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
        
    }
}
