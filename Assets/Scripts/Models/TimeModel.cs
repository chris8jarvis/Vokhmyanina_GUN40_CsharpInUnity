using System;
using Core.Utils;
using Cysharp.Threading.Tasks;
using Models.Interfaces;
using UniRx;
using System.Threading;

namespace Models
{
    public sealed class TimeModel:  ITimeModel
    {
        private readonly ReactiveProperty<int> _gameTime = new();
        public IObservable<int> GameTime =>  _gameTime;
        private CancellationTokenSource _cts;
        public TimeModel()
        {
            _cts = new CancellationTokenSource();
        }

        public void Initialize() => CountTime(_cts.Token).Forget();

        private async UniTask CountTime(CancellationToken token)
        {
            while (true)
            {
                token.ThrowIfCancellationRequested();
                await UniTask.Delay(NumericConstants.One * 1000, cancellationToken: token);
                _gameTime.Value++;
            }
        }
        public void Dispose()
        {
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = null;
        }
    }
}