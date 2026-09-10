using Models;
using Models.Interfaces;
using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

namespace Presenters.Menu
{
    public sealed class BonusUIView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _starCountText;
        [SerializeField] private TextMeshProUGUI _heartCountText;
        
        private IBonusModel _bonusModel;
        private readonly CompositeDisposable _disposable = new();

        [Inject]
        public void Inject(IBonusModel bonusModel)
        {
            _bonusModel = bonusModel;
        }

        private void Start()
        {
            if (_bonusModel == null)
            {
                Debug.LogError("BonusModel is null in BonusUIView");
                return;
            }

            _bonusModel.CollectedBonuses
                .ObserveAdd()
                .Subscribe(_ => UpdateUI())
                .AddTo(_disposable);

            _bonusModel.CollectedBonuses
                .ObserveRemove()
                .Subscribe(_ => UpdateUI())
                .AddTo(_disposable);

            _bonusModel.CollectedBonuses
                .ObserveReset()
                .Subscribe(_ => UpdateUI())
                .AddTo(_disposable);

            UpdateUI();
        }

        private void UpdateUI()
        {
            int starCount = 0;
            int heartCount = 0;

            foreach (var bonus in _bonusModel.CollectedBonuses)
            {
                if (bonus == Bonuses.Star) starCount++;
                else if (bonus == Bonuses.Heart) heartCount++;
            }

            if (_starCountText != null) _starCountText.text = $"Stars: {starCount}";
            if (_heartCountText != null) _heartCountText.text = $"Hearts: {heartCount}";
        }

        private void OnDestroy()
        {
            _disposable.Dispose();
        }
    }
}