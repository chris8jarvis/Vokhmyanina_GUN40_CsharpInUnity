using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    [Header("Restart Settings")]
    [SerializeField] private GameObject restartPanel;
    [SerializeField] private Image restartFillImage;
    [SerializeField] private float restartHoldTime = 2f;
    
    private Controls controls;
    private float currentHoldTime = 0f;
    private bool isHoldingRestart = false;
    
    private void Awake()
    {
        controls = new Controls();
    }
    
    private void OnEnable()
    {
        controls.Game.Restart.performed += OnRestartPerformed;
        controls.Game.Restart.canceled += OnRestartCanceled;
        controls.Game.Enable();
    }
    
    private void OnDisable()
    {
    }
    
    private void Start()
    {
        if (restartPanel != null)
            restartPanel.SetActive(false);
    }
    
    private void Update()
    {
        if (isHoldingRestart)
        {
            currentHoldTime += Time.deltaTime;
            
            float fillAmount = currentHoldTime / restartHoldTime;
            restartFillImage.fillAmount = Mathf.Clamp01(fillAmount);
            
            if (currentHoldTime >= restartHoldTime)
            {
                RestartScene();
            }
        }
    }
    
    private void OnRestartPerformed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        isHoldingRestart = true;
        currentHoldTime = 0f;
        
        if (restartPanel != null)
        {
            restartPanel.SetActive(true);
            restartFillImage.fillAmount = 0f;
        }
    }
    
    private void OnRestartCanceled(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        isHoldingRestart = false;
        currentHoldTime = 0f;
        
        if (restartPanel != null)
        {
            restartPanel.SetActive(false);
        }
    }
    
    private void RestartScene()
    {
        isHoldingRestart = false;
        currentHoldTime = 0f;
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnDestroy()
    {
        if (controls != null)
        {
            controls.Game.Restart.performed -= OnRestartPerformed;
            controls.Game.Restart.canceled -= OnRestartCanceled;
            controls.Game.Disable();
            controls.Dispose();
        }
    }
}
