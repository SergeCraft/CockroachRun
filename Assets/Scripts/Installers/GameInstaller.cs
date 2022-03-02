using Config;
using Main;
using UnityEngine;
using Zenject;

public class GameInstaller: MonoInstaller<GameInstaller>
{
    
    public override void InstallBindings()
    {
        Debug.Log("Container setting up...");

        SetupBindings();
        SetupSignalBus();
        
        Debug.Log("Container was set");
        
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
    }
}

