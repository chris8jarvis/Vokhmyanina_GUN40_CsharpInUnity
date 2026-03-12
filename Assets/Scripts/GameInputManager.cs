using UnityEngine;
using UnityEngine.InputSystem;

public class GameInputManager : MonoBehaviour
{
    private Controls controls;
    
    private void Awake()
    {
        controls = new Controls();
    }
    
    private void OnEnable()
    {
        controls.Game.Restart.performed += OnRestartPerformed;
        controls.Game.Enable();
    }
    
    private void OnDisable()
    {
        controls.Game.Restart.performed -= OnRestartPerformed;
        controls.Game.Disable();
    }
    
    private void OnRestartPerformed(InputAction.CallbackContext context)
    {
        Debug.Log("Restart pressed!");
    }
}
