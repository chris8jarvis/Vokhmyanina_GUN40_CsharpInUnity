using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BowlingScore : MonoBehaviour
{
    public Text scoreText;           
    public Text currentThrowText; 
    public Text frameStatusText; 
    public Pin[] pins;
    
    private Vector3[] initialPinPositions;
    private Quaternion[] initialPinRotations;
    private Rigidbody[] pinRigidbodies;
    
    private int totalScore = 0;
    private int currentFrame = 1;
    private int throwNumber = 1;
    private int pinsDownThisThrow = 0;
    private int pinsDownFirstThrow = 0;
    
    private int[] throws = new int[21];
    private int currentThrow = 0;
    
    private bool isWaitingForReset = false;
    private bool isFrameComplete = false;
    
    public System.Action OnRoundReady; // Событие готовности к следующему броску

    void Start()
    {
        initialPinPositions = new Vector3[pins.Length];
        initialPinRotations = new Quaternion[pins.Length];
        pinRigidbodies = new Rigidbody[pins.Length];
        
        for (int i = 0; i < pins.Length; i++)
        {
            initialPinPositions[i] = pins[i].transform.position;
            initialPinRotations[i] = pins[i].transform.rotation;
            pinRigidbodies[i] = pins[i].GetComponent<Rigidbody>();
        }

        foreach (Pin pin in pins)
        {
            pin.onPinFall.AddListener(() => OnPinFall());
        }
        
        UpdateUI();
    }

    void OnPinFall()
    {
        pinsDownThisThrow++;
        UpdateUI();
    }

    public void EndThrow()
    {
        if (isWaitingForReset) return;
        
        StartCoroutine(ShowThrowResult());
    }
    
    IEnumerator ShowThrowResult()
    {
        isWaitingForReset = true;
        
        frameStatusText.text = $"Knocked down: {pinsDownThisThrow} pins";
        frameStatusText.color = Color.yellow;
        
        yield return new WaitForSeconds(1.5f);
        
        throws[currentThrow] = pinsDownThisThrow;
        currentThrow++;
        
        ProcessFrame();
        
        UpdateUI();
        
        yield return new WaitForSeconds(2f);
        
        if (isFrameComplete)
        {
            frameStatusText.text = "Pins reset";
            yield return new WaitForSeconds(1f);
            
            ResetPins();
            isFrameComplete = false;
        }
        
        isWaitingForReset = false;
        OnRoundReady?.Invoke();
    }
    
    void ProcessFrame()
{
    if (throwNumber == 1)
    {
        pinsDownFirstThrow = pinsDownThisThrow;
        
        if (pinsDownThisThrow == 10) // STRIKE
        {
            if (frameStatusText != null)
            {
                frameStatusText.text = "Strike";
                frameStatusText.color = Color.green;
            }
            currentFrame++;
            throwNumber = 1;
            isFrameComplete = true;
        }
        else
        {
            if (frameStatusText != null)
            {
                frameStatusText.text = $"Throw 1: {pinsDownThisThrow} pins";
                frameStatusText.color = Color.white;
            }
            throwNumber = 2;
            isFrameComplete = false;
        }
    }
    else 
    {
        int totalInFrame = pinsDownFirstThrow + pinsDownThisThrow;
        
        // Отладка в консоль
        Debug.Log($"Первый бросок: {pinsDownFirstThrow}, Второй: {pinsDownThisThrow}, Сумма: {totalInFrame}");
        
        if (totalInFrame == 10) // SPARE
        {
            if (frameStatusText != null)
            {
                frameStatusText.text = "Spare";
                frameStatusText.color = Color.cyan;
            }
        }
        else
        {
            if (frameStatusText != null)
            {
                frameStatusText.text = "Total: " + pinsDownFirstThrow.ToString() + " + " + pinsDownThisThrow.ToString() + " = " + totalInFrame.ToString() + " points";
                frameStatusText.color = Color.white;
            }
        }
        
        currentFrame++;
        throwNumber = 1;
        isFrameComplete = true;
    }
    
    CalculateTotalScore();
    
    pinsDownThisThrow = 0;
    
    if (currentFrame > 10)
    {
        if (frameStatusText != null)
        {
            frameStatusText.text = "Game over";
        }
        if (scoreText != null)
        {
            scoreText.text = $"Final score: {totalScore}";
        }
    }
}
    
    void CalculateTotalScore()
    {
        totalScore = 0;
        int throwIndex = 0;
        
        for (int frame = 0; frame < 10; frame++)
        {
            if (throwIndex >= currentThrow) break;
            
            if (throws[throwIndex] == 10) 
            {
                int bonus1 = (throwIndex + 1 < currentThrow) ? throws[throwIndex + 1] : 0;
                int bonus2 = (throwIndex + 2 < currentThrow) ? throws[throwIndex + 2] : 0;
                totalScore += 10 + bonus1 + bonus2;
                throwIndex++;
            }
            else
            {
                int firstThrow = throws[throwIndex];
                int secondThrow = (throwIndex + 1 < currentThrow) ? throws[throwIndex + 1] : 0;
                
                if (firstThrow + secondThrow == 10 && secondThrow > 0)
                {
                    int bonus = (throwIndex + 2 < currentThrow) ? throws[throwIndex + 2] : 0;
                    totalScore += 10 + bonus;
                }
                else
                {
                    totalScore += firstThrow + secondThrow;
                }
                throwIndex += 2;
            }
        }
    }

    void ResetPins()
    {
        for (int i = 0; i < pins.Length; i++)
        {
            pins[i].transform.position = initialPinPositions[i];
            pins[i].transform.rotation = initialPinRotations[i];
            
            Rigidbody rb = pinRigidbodies[i];
            if (rb != null)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.useGravity = false;
            }
            
            Pin pinScript = pins[i].GetComponent<Pin>();
            if (pinScript != null)
                pinScript.ResetPin();
        }
        Invoke("EnableGravity", 0.5f);
    }

    void EnableGravity()
    {
        for (int i = 0; i < pins.Length; i++)
        {
            Rigidbody rb = pinRigidbodies[i];
            if (rb != null)
            {
                rb.useGravity = true;
            }
        }
    }
    
    IEnumerator SmoothReset(GameObject pin, Vector3 targetPos, Quaternion targetRot)
    {
        Vector3 startPos = pin.transform.position;
        Quaternion startRot = pin.transform.rotation;
        float elapsed = 0;
        float duration = 0.3f;
        
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            pin.transform.position = Vector3.Lerp(startPos, targetPos, t);
            pin.transform.rotation = Quaternion.Slerp(startRot, targetRot, t);
            yield return null;
        }
        
        pin.transform.position = targetPos;
        pin.transform.rotation = targetRot;
    }
    
    void UpdateUI()
    {
        if (scoreText != null)
        {
            scoreText.text = $"Score: {totalScore}";
        }
        
        if (currentThrowText != null)
        {
            currentThrowText.text = $"This throw: {pinsDownThisThrow} pins";
        }
    }
    
    public int GetCurrentThrowNumber() => throwNumber;
    public bool IsWaiting() => isWaitingForReset;
}