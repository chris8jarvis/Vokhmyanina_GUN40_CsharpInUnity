using UnityEngine;
using Zenject;
using UnityEngine.InputSystem;
using Controllers;
using Units;

public class SceneInstaller : MonoInstaller
{

    [SerializeField] private InputActionAsset inputActions;
    [SerializeField] private BattleController battleController;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Battlefield battlefield;

    public override void InstallBindings()
    {
        Container.Bind<InputActionAsset>()
                 .FromInstance(inputActions)
                 .AsSingle();

        Container.Bind<BattleController>()
                 .FromInstance(battleController)
                 .AsSingle();
        
        Container.Bind<PlayerController>()
                 .FromInstance(playerController)
                 .AsSingle();
        
        Container.Bind<Battlefield>()
                 .FromInstance(battlefield)
                 .AsSingle();

        Debug.Log("SceneInstaller: All bindings registered");
    }
}
