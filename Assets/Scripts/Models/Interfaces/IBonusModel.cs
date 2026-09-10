using System;
using UniRx;

namespace Models.Interfaces
{
    public interface IBonusModel : IDisposable
    {
        IReadOnlyReactiveCollection<Bonuses> CollectedBonuses { get; }
        IReadOnlyReactiveCollection<Bonuses> SavedBonuses { get; }
        void AddBonus(Bonuses bonus);
        void SaveAndClearCurrentSession();
        void ClearCurrentSession(); 
        void LoadSavedBonuses();
    }
}