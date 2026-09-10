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
        public IReadOnlyReactiveCollection<Bonuses> CollectedBonuses => _collectedBonuses;

        private readonly ISaveLoadDataHandler _saveLoadHandler;
        private const string SAVE_KEY = "CollectedBonuses";

        public BonusModel(ISaveLoadDataHandler saveLoadHandler)
        {
            _saveLoadHandler = saveLoadHandler;
            LoadBonuses();
        }

        public void AddBonus(Bonuses bonus)
        {
            _collectedBonuses.Add(bonus);
            SaveBonuses();
        }

        public void ClearBonuses()
        {
            _collectedBonuses.Clear();
            SaveBonuses();
        }

        public void SaveBonuses()
        {
            string bonusString = string.Join(",", _collectedBonuses.Select(b => ((int)b).ToString()));
            _saveLoadHandler.SaveString(SAVE_KEY, bonusString);
        }

        public void LoadBonuses()
        {
            if (_saveLoadHandler.TryLoadString(SAVE_KEY, out string bonusString) && !string.IsNullOrEmpty(bonusString))
            {
                _collectedBonuses.Clear();
                string[] bonusValues = bonusString.Split(',');
                foreach (string value in bonusValues)
                {
                    if (int.TryParse(value, out int bonusIndex))
                    {
                        _collectedBonuses.Add((Bonuses)bonusIndex);
                    }
                }
            }
        }

        public void Dispose()
        {
            _collectedBonuses?.Dispose();
        }
    }
}