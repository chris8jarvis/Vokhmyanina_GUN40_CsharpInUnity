using Models;
using UnityEngine;
using Models.Interfaces;

public class BonusCollector : MonoBehaviour
{
    private Bonuses _bonusType;
    private IBonusModel _bonusModel;
    private bool _isCollected;

    public void Initialize(Bonuses bonusType, IBonusModel bonusModel)
    {
        _bonusType = bonusType;
        _bonusModel = bonusModel;
        _isCollected = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_isCollected) return;
        
        if (other.CompareTag("Player"))
        {
            _isCollected = true;
            _bonusModel.AddBonus(_bonusType);
            
            Destroy(gameObject);
        }
    }
}