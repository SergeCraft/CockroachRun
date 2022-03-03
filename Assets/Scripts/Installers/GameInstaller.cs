using Config;
using Main;
using Player;
using UnityEngine;
using Zenject;

public class GameInstaller: MonoInstaller<GameInstaller>
{
    
    public override void InstallBindings()
    {
        Debug.Log("Container setting up...");

        SetupBindings();
        SetupSignalBus();
        InstantiatePlayer();
        
        Debug.Log("Container was set");
        
    }

    private void InstantiatePlayer()
    {
        Container.InstantiatePrefabResource("SergeCraft/Prefabs/Player");
    }

    private void SetupBindings()
    {
        Container.BindInterfacesTo<Game>().AsSingle();
        Container.Bind<IConfigManager>().To<HardcodeGameConfigManager>().AsSingle();
    }

    private void SetupSignalBus()
    {
        SignalBusInstaller.Install(Container);
        
        Container.DeclareSignal<ComponentsLoadedSignal>();
        Container.DeclareSignal<GameStartedSignal>();
        Container.DeclareSignal<GameoverSignal>();
        Container.DeclareSignal<PlayerMoveToBottomSignal>();
        Container.DeclareSignal<PlayerMoveToTopSignal>();
        Container.DeclareSignal<PlayerStateChangedSignal>();
    }
}

