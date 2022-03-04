using Config;
using Level;
using Main;
using Player;
using UnityEngine;
using Zenject;

public class GameInstaller: MonoInstaller<GameInstaller>
{

    public GameObject FloorPrefab;
    public GameObject PlayerPrefab;
    
    public override void InstallBindings()
    {
        Debug.Log("Container setting up...");

        SetupPrefabs();
        SetupBindings();
        SetupSignalBus();
        InstantiatePlayer();
        
        Debug.Log("Container was set");
        
    }

    private void SetupPrefabs()
    {
        if (FloorPrefab == null) 
            FloorPrefab = Resources.Load<GameObject>("SergeCraft/Prefabs/Floor");
        if (PlayerPrefab == null) 
            PlayerPrefab = Resources.Load<GameObject>("SergeCraft/Prefabs/Player");
    }

    private void InstantiatePlayer()
    {
        Container.InstantiatePrefab(PlayerPrefab);
    }

    private void SetupBindings()
    {
        Container.BindInterfacesTo<Game>().AsSingle();
        Container.BindInstance(new GameConfig()
            {
                GameSpeed = 3.0f,
                TopFloorDefaultY = 3.0f,
                BottomFloorDefaultY = -3.0f
            }).AsSingle();
        Container.BindInterfacesTo<SimplePlayerController>().AsSingle();
        Container.BindInterfacesTo<SimpleLevelController>().AsSingle();
        
        Container.BindInstance(new FloorView.ConstructOptions (
            new Vector2(0.0f, 0.0f),
            new Vector2(0.0f, 0.0f),
            "Untagged"));
        Container.BindFactory<FloorView.ConstructOptions, FloorView, FloorView.Factory>()
            .FromComponentInNewPrefab(FloorPrefab);
    }

    private void SetupSignalBus()
    {
        SignalBusInstaller.Install(Container);
        
        Container.DeclareSignal<ComponentsLoadedSignal>();
        Container.DeclareSignal<GameStartedSignal>();
        Container.DeclareSignal<GameoverSignal>();
        Container.DeclareSignal<GamePausedSignal>();
        Container.DeclareSignal<GameFlipGravitySignal>();
        Container.DeclareSignal<PlayerStateChangedSignal>();
        Container.DeclareSignal<PlayerHitSignal>();
        Container.DeclareSignal<FloorReachedPositionSignal>();
    }
}

