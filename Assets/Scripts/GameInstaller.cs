using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Controls controls = new Controls();
        
        // Регистрируем сам Controls (синглтон)
        Container.Bind<Controls>().FromInstance(controls).AsSingle();
        
        // Регистрируем карту GameActions (чтобы можно было получить controls.Game)
        Container.Bind<Controls.GameActions>().FromInstance(controls.Game).AsSingle();
        
        // Включаем Controls (чтобы они работали)
        controls.Game.Enable();
    }
}
