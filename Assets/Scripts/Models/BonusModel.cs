using System;
using System.Collections.Generic;
using System.Linq;
using Models.Interfaces;
using UniRx;
using Core.SaveLoad;

namespace Models
{
    public sealed class BonusModel : IBonusModel
    {
        private readonly ReactiveCollection<Bonuses> _collectedBonuses = new();
        private readonly ReactiveCollection<Bonuses> _savedBonuses = new();
        
        public IReadOnlyReactiveCollection<Bonuses> CollectedBonuses => _collectedBonuses;
        public IReadOnlyReactiveCollection<Bonuses> SavedBonuses => _savedBonuses;

        private readonly ISaveLoadDataHandler _saveLoadHandler;
        private const string SAVE_KEY = "SavedBonuses";

        public BonusModel(ISaveLoadDataHandler saveLoadHandler)
        {
            _saveLoadHandler = saveLoadHandler;
            LoadSavedBonuses();
        }

        public void AddBonus(Bonuses bonus)
        {
            _collectedBonuses.Add(bonus);
        }

        public void ClearCurrentSession()
        {
            _collectedBonuses.Clear();
        }

         public void SaveAndClearCurrentSession()
        {
            string bonusString = string.Join(",", _collectedBonuses.Select(b => ((int)b).ToString()));
            _saveLoadHandler.SaveString(SAVE_KEY, bonusString);

            _savedBonuses.Clear();
            foreach (var bonus in _collectedBonuses)
            {
                _savedBonuses.Add(bonus);
            }
            _collectedBonuses.Clear();
        }

        public void LoadSavedBonuses()
        {
            if (_saveLoadHandler.TryLoadString(SAVE_KEY, out string bonusString) && !string.IsNullOrEmpty(bonusString))
            {
                _savedBonuses.Clear();
                string[] bonusValues = bonusString.Split(',');
                foreach (string value in bonusValues)
                {
                    if (int.TryParse(value, out int bonusIndex))
                    {
                        _savedBonuses.Add((Bonuses)bonusIndex);
                    }
                }
            }
        }

        public void Dispose()
        {
            _collectedBonuses?.Dispose();
            _savedBonuses?.Dispose();
        }
    }
}