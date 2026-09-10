using System;
using UniRx;

namespace Models.Interfaces
{
    public interface IBonusModel : IDisposable
    {
        IReadOnlyReactiveCollection<Bonuses> CollectedBonuses { get; }
        void AddBonus(Bonuses bonus);
        void ClearBonuses();
        void SaveBonuses();
        void LoadBonuses();
    }
}