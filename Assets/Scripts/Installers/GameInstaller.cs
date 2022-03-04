using Config;
using GUI;
using Level;
using Main;
using Player;
using UnityEngine;
using Zenject;

public class GameInstaller: MonoInstaller<GameInstaller>
{

    public GameObject FloorPrefab;
    public GameObject PlayerPrefab;
    public GameObject UIPrefab;
    
    public override void InstallBindings()
    {
        Debug.Log("Container setting up...");

        SetupPrefabs();
        SetupBindings();
        SetupSignalBus();
        InstantiatePrefabs();
        
        Debug.Log("Container was set");
        
    }

    private void SetupPrefabs()
    {
        if (FloorPrefab == null) 
            FloorPrefab = Resources.Load<GameObject>("SergeCraft/Prefabs/Floor");
        if (PlayerPrefab == null) 
            PlayerPrefab = Resources.Load<GameObject>("SergeCraft/Prefabs/Player");
        if (UIPrefab == null) 
            UIPrefab = Resources.Load<GameObject>("SergeCraft/Prefabs/SimpleUI");
    }

    private void InstantiatePrefabs()
    {
        Container.InstantiatePrefab(PlayerPrefab);
    }

    private void SetupBindings()
    {
        Container.BindInterfacesTo<Game>().AsSingle();
        Container.BindInstance(new GameConfig()
            {
                GameSpeed = 3.0f,
                TopFloorDefaultY = 2.0f,
                BottomFloorDefaultY = -2.0f,
                FloorSpaceXMin = 1.5f,
                FloorSpaceXMax= 2.5f,
                FloorScaleXMin = 2.0f,
                FloorScaleXMax = 10.0f,
                FloorScaleYMin = 0.2f,
                FloorScaleYMax = 0.5f
            }).AsSingle();
        Container.Bind<Score>().AsSingle();
        Container.BindInterfacesTo<SimplePlayerController>().AsSingle();
        Container.BindInterfacesTo<SimpleLevelController>().AsSingle();
        Container.BindInterfacesTo<SimpleUIView>().FromComponentInNewPrefab(UIPrefab).AsSingle();

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
        
        Container.DeclareSignal<GameStateChangedSignal>();
        Container.DeclareSignal<GameStateChangeRequestedSignal>();
        Container.DeclareSignal<GameFlipGravitySignal>();
        Container.DeclareSignal<PlayerStateChangedSignal>();
        Container.DeclareSignal<PlayerHitSignal>();
        Container.DeclareSignal<PlayerLeftSignal>();
        Container.DeclareSignal<FloorReachedPositionSignal>();
    }
}

