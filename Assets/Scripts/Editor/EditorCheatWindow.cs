// using UnityEditor;
// using UnityEngine;
// using UnityEngine.InputSystem;
// using Units;

// public class EditorCheatWindow : EditorWindow
// {
//     private EditorControls _inputActions;
    
//     [MenuItem("Netologia/Windows/Editor Cheat Window")]
//     public static void ShowWindow()
//     {
//         GetWindow<EditorCheatWindow>("Cheat Codes");
//     }
    
//     private void OnEnable()
//     {
//         EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        
//         _inputActions = new EditorControls();
//         _inputActions.Enable();
        
//         _inputActions.Cheats.NextTurn.performed += OnNextTurnPerformed;
//         _inputActions.Cheats.Kill.performed += OnKillPerformed;
//     }
    
//     private void OnDisable()
//     {
//         EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
        
//         if (_inputActions != null)
//         {
//             _inputActions.Cheats.NextTurn.performed -= OnNextTurnPerformed;
//             _inputActions.Cheats.Kill.performed -= OnKillPerformed;
//             _inputActions.Disable();
//         }
//     }
    
//     private void OnPlayModeStateChanged(PlayModeStateChange state)
//     {
//         if (state == PlayModeStateChange.EnteredPlayMode)
//         {
//             Debug.Log("Cheat Window is active: press 1 for next turn, 2 for kill");
//         }
//     }
    
//     private void OnNextTurnPerformed(InputAction.CallbackContext obj)
//     {
//         if (!EditorApplication.isPlaying) return;

//          Debug.Log("Next Turn");
        
        
//         var battlefield = FindAnyObjectByType<Battlefield>();
//         if (battlefield != null)
//         {
//             Debug.Log("Found Battlefield, next turn could be made");
//         }
//     }
    
//     private void OnKillPerformed(InputAction.CallbackContext obj)
//     {
//         if (!EditorApplication.isPlaying) return;
        
//         Debug.Log("Killed");

//         var unit = FindAnyObjectByType<Unit>();
//         if (unit != null)
//         {
//             Destroy(unit.gameObject);
//             Debug.Log("Unit destroyed");
//         }
//     }
    
//     private void OnGUI()
//     {
//         GUILayout.Label("Cheat Codes", EditorStyles.boldLabel);
//         GUILayout.Label("1 - Next Turn");
//         GUILayout.Label("2 - Kill Enemy");
        
//         if (!EditorApplication.isPlaying)
//         {
//             EditorGUILayout.HelpBox("Enter Play Mode to use Cheats", MessageType.Info);
//         }
//     }
// }
