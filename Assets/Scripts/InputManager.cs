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
        // Подписываемся на события нажатия и отпускания
        controls.Game.Restart.performed += OnRestartPerformed;
        controls.Game.Restart.canceled += OnRestartCanceled;
        controls.Game.Enable();
    }
    
    private void OnDisable()
    {
    }
    
    private void Start()
    {
        // В начале игры панель выключена
        if (restartPanel != null)
            restartPanel.SetActive(false);
    }
    
    private void Update()
    {
        if (isHoldingRestart)
        {
            // Увеличиваем время удержания
            currentHoldTime += Time.deltaTime;
            
            // Обновляем fillAmount (от 0 до 1)
            float fillAmount = currentHoldTime / restartHoldTime;
            restartFillImage.fillAmount = Mathf.Clamp01(fillAmount);
            
            // Проверяем, заполнилась ли шкала
            if (currentHoldTime >= restartHoldTime)
            {
                RestartScene();
            }
        }
    }
    
    private void OnRestartPerformed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        // Начали удерживать кнопку
        isHoldingRestart = true;
        currentHoldTime = 0f;
        
        // Включаем панель
        if (restartPanel != null)
        {
            restartPanel.SetActive(true);
            restartFillImage.fillAmount = 0f;
        }
    }
    
    private void OnRestartCanceled(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        // Отпустили кнопку раньше времени
        isHoldingRestart = false;
        currentHoldTime = 0f;
        
        // Выключаем панель
        if (restartPanel != null)
        {
            restartPanel.SetActive(false);
        }
    }
    
    private void RestartScene()
    {
        // Сброс состояния
        isHoldingRestart = false;
        currentHoldTime = 0f;
        
        // Перезагрузка текущей сцены
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
