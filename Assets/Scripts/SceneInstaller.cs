using Commands;
using UnityEngine;
using Zenject;
using UnityEngine.InputSystem;
using Controllers;

public class SceneInstaller : MonoInstaller
{

    [SerializeField] private InputActionAsset inputActions;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Battlefield battlefield;
    [SerializeField] private CellPaletteSettings cellPalette;
    

    public override void InstallBindings()
    {
        Container.Bind<CellPaletteSettings>()
            .FromInstance(cellPalette)
            .AsSingle();

        Container.Bind<Battlefield>()
            .FromInstance(battlefield)
            .AsSingle();

        Container.Bind<GameStateSystem>()
            .AsSingle();

        Container.Bind<PlayerController>()
            .FromInstance(playerController)
            .AsSingle();

        Container.BindInterfacesAndSelfTo<SelectCommand>()
            .AsSingle();

        Container.BindInterfacesAndSelfTo<MoveCommand>()
            .AsSingle();

        Container.BindInterfacesAndSelfTo<BattleController>()
            .AsSingle()
            .WithArguments(inputActions);

        Debug.Log("SceneInstaller: All bindings registered");
    }
}
