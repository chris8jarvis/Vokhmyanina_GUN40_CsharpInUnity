using Zenject;
using UnityEngine;
using UI;
using Models;
using Presenters;

namespace Installers
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private HealthView _healthView;

        public override void InstallBindings()
        {
            Container.Bind<PlayerModel>().AsSingle();
            Container.Bind<HealthView>().FromInstance(_healthView).AsSingle();
            Container.BindInterfacesTo<PlayerPresenter>().AsSingle();

            // TODO: radar and savedata bindings needed
        }
    }
}