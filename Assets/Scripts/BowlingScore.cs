using UnityEngine;
using UnityEngine.UI;

public class BowlingScore : MonoBehaviour
{
    public Text scoreText;
    public Pin[] pins;
    
    private Vector3[] initialPinPositions;
    private Quaternion[] initialPinRotations;
    private Rigidbody[] pinRigidbodies;
    
    private int totalScore = 0;
    private int currentFrame = 1;
    private int throwNumber = 1;
    private int pinsDownThisThrow = 0;
    private int pinsDownPreviousThrow = 0;
    
    private int[] throws = new int[21];
    private int currentThrow = 0;
    
    private bool needResetPins = false;

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
        throws[currentThrow] = pinsDownThisThrow;
        currentThrow++;
        
        CalculateTotalScore();
        UpdateUI();
        
        if (throwNumber == 1)
        {
            if (pinsDownThisThrow == 10) // STRIKE
            {
                currentFrame++;
                throwNumber = 1;
                needResetPins = true;
            }
            else
            {
                pinsDownPreviousThrow = pinsDownThisThrow;
                throwNumber = 2;
                needResetPins = false;
            }
        }
        else 
        {
            currentFrame++;
            throwNumber = 1;
            needResetPins = true; 
        }
        
        if (needResetPins)
        {
            ResetPins();
        }
        
        pinsDownThisThrow = 0;
        
        if (currentFrame > 10)
        {
            scoreText.text = $"FINAL SCORE: {totalScore}";
        }
    }
    
    void CalculateTotalScore()
    {
        totalScore = 0;
        int throwIndex = 0;
        
        for (int frame = 0; frame < 10; frame++)
        {
            if (throwIndex >= currentThrow) break;
            
            if (throws[throwIndex] == 10) // STRIKE
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
                
                if (firstThrow + secondThrow == 10 && secondThrow > 0) // SPARE
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
            
            if (pinRigidbodies[i] != null)
            {
                pinRigidbodies[i].velocity = Vector3.zero;
                pinRigidbodies[i].angularVelocity = Vector3.zero;
            }
            
            Pin pinScript = pins[i].GetComponent<Pin>();
            if (pinScript != null)
                pinScript.ResetPin();
        }
    }
    
    public bool CanThrowAgain()
    {
        return throwNumber == 2 && currentFrame <= 10;
    }
    
    void UpdateUI()
    {
        if (scoreText != null)
        {
            string info = $"Score: {totalScore}\nFrame: {currentFrame}";
            if (throwNumber == 1)
                info += "\nThrow: 1";
            else
                info += $"\nThrow: 2 (first: {pinsDownPreviousThrow})";
            scoreText.text = info;
        }
    }
}