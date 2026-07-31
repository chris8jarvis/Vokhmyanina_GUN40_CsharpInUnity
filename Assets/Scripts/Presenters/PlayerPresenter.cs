using Zenject;
using UniRx;
using Models;
using UI;

namespace Presenters
{
    public class PlayerPresenter : IInitializable
    {
        private readonly PlayerModel _model;
        private readonly HealthView _view;

        public PlayerPresenter(PlayerModel model, HealthView view)
        {
            _model = model;
            _view = view;
        }

        public void Initialize()
        {
            _model.Health.Subscribe(_view.UpdateHealth).AddTo(_view);
            _model.Score.Subscribe(_view.UpdateScore).AddTo(_view);
            _model.IsGameOver.Subscribe(_view.ShowGameOver).AddTo(_view);
        }
    }
}