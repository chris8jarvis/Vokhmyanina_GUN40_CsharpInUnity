using Presenters.Menu;
using Zenject;

namespace Installers
{
    public sealed class MainInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<SavedBonusUIView>().FromComponentInHierarchy().AsSingle();
        }
    }
}