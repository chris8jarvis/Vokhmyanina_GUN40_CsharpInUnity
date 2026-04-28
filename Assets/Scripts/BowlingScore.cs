using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class BowlingScore : MonoBehaviour
{
    public Text scoreText;
    public Text frameInfoText;
    public Pin[] pins;
    
    private Vector3[] initialPinPositions;
    private Quaternion[] initialPinRotations;
    private Rigidbody[] pinRigidbodies;
    
    private int currentScore = 0;
    private int currentFrame = 1;
    private int throwNumber = 1; 
    private int pinsDownThisFrame = 0;
    private bool waitingForBonus = false;
    private int bonusRemaining = 0;
    private int bonusScore = 0;
    
    private Dictionary<int, int> frameScores = new Dictionary<int, int>();
    private Dictionary<int, string> frameTypes = new Dictionary<int, string>();

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
        pinsDownThisFrame++;
        UpdateUI();
    }

    public void EndThrow()
    {
        int pinsThisThrow = pinsDownThisFrame;
        
        if (throwNumber == 1)
        {
            if (pinsThisThrow == 10) // STRIKE
            {
                frameTypes[currentFrame] = "Strike";
                bonusRemaining = 2;
                waitingForBonus = true;
                currentScore += 10;
                EndFrame();
            }
            else
            {
                frameTypes[currentFrame] = "Normal";
                throwNumber = 2;
                frameScores[currentFrame] = pinsThisThrow;
            }
        }
        else 
        {
            int previousThrow = frameScores.ContainsKey(currentFrame) ? frameScores[currentFrame] : 0;
            int totalInFrame = pinsThisThrow + previousThrow;
            
            if (totalInFrame == 10) // SPARE
            {
                frameTypes[currentFrame] = "Spare";
                bonusRemaining = 1;
                waitingForBonus = true;
                currentScore += 10;
            }
            else
            {
                currentScore += pinsThisThrow;
            }
            EndFrame();
        }
        
        UpdateUI();
    }
    
    void EndFrame()
    {
        currentFrame++;
        throwNumber = 1;
        pinsDownThisFrame = 0;
        
        if (currentFrame > 10)
        {
            frameInfoText.text = "Game Over! Final Score: " + currentScore;
        }
    }
    
    public void ApplyBonus(int bonusPins)
    {
        if (waitingForBonus && bonusRemaining > 0)
        {
            bonusScore += bonusPins;
            bonusRemaining--;
            if (bonusRemaining == 0)
            {
                currentScore += bonusScore;
                bonusScore = 0;
                waitingForBonus = false;
            }
        }
        UpdateUI();
    }
    
    public void ResetPins()
    {
        for (int i = 0; i < pins.Length; i++)
        {
            pins[i].transform.position = initialPinPositions[i];
            pins[i].transform.rotation = initialPinRotations[i];
            
            if (pinRigidbodies[i] != null)
            {
                pinRigidbodies[i].velocity = Vector3.zero;
                pinRigidbodies[i].angularVelocity = Vector3.zero;
                pinRigidbodies[i].isKinematic = false;
            }
            
            Pin pinScript = pins[i].GetComponent<Pin>();
            if (pinScript != null)
                pinScript.ResetPin();
        }
        
        pinsDownThisFrame = 0;
        UpdateUI();
    }
    
    void UpdateUI()
    {
        scoreText.text = $"Score: {currentScore}\nFrame: {currentFrame}\nThrow: {throwNumber}\nPins down: {pinsDownThisFrame}";
        
        if (frameTypes.ContainsKey(currentFrame))
            frameInfoText.text = $"Frame {currentFrame}: {frameTypes[currentFrame]}";
    }
}