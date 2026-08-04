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
        [SerializeField] private RadarView _radarView;
        [SerializeField] private Transform _player;

        public override void InstallBindings()
        {
            Container.Bind<PlayerModel>().AsSingle();
            Container.Bind<HealthView>().FromInstance(_healthView).AsSingle();
            Container.Bind<RadarView>().FromInstance(_radarView).AsSingle();
            Container.Bind<Transform>().FromInstance(_player).WhenInjectedInto<RadarPresenter>();
            Container.BindInterfacesTo<RadarPresenter>().AsSingle();
            Container.BindInterfacesTo<PlayerPresenter>().AsSingle();

        }
    }
}