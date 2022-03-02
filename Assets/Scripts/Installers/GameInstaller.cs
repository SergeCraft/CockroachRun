using UnityEngine;
using Zenject;

public class GameInstaller: MonoInstaller<GameInstaller>
{
    public override void InstallBindings()
    {
        Debug.Log("Container setting up...");
        
        SetupSignalBus();
        
        Debug.Log("Container was set");
        
    }

    private void SetupSignalBus()
    {
        SignalBusInstaller.Install(Container);
    }
}

