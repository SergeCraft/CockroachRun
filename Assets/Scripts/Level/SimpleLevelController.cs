using System;
using Config;
using Main;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Level
{
    public class SimpleLevelController : IDisposable, ILevelController
    {
        private FloorView.Factory _factory;
        private SignalBus _signalBus;
        private GameConfig _config;

        public SimpleLevelController(FloorView.Factory factory, SignalBus signalBus, GameConfig config)
        {
            _factory = factory;
            _signalBus = signalBus;
            _config = config;
           
            SubscribeToSignalBus();
        }

        public void Dispose()
        {
            UnsubscribeFromSignalBus();
        }

        public void Freeze()
        {
            foreach (var floorObj in GameObject.FindGameObjectsWithTag("TopFloor"))
            {
                floorObj.GetComponent<FloorView>().Freeze();
            };
            
            foreach (var floorObj in GameObject.FindGameObjectsWithTag("BottomFloor"))
            {
                floorObj.GetComponent<FloorView>().Freeze();
            }
        }

        public void Unfreeze()
        {
            foreach (var floorObj in GameObject.FindGameObjectsWithTag("TopFloor"))
            {
                floorObj.GetComponent<FloorView>().Unfreeze();
            };
            
            foreach (var floorObj in GameObject.FindGameObjectsWithTag("BottomFloor"))
            {
                floorObj.GetComponent<FloorView>().Unfreeze();
            }
        }
        
        private void UnsubscribeFromSignalBus()
        {
            _signalBus.Unsubscribe<FloorReachedPositionSignal>(OnFloorReachedPosition);
            _signalBus.Unsubscribe<GameStateChangedSignal>(OnGameStateChanged);
        }

        private void SubscribeToSignalBus()
        {
            _signalBus.Subscribe<FloorReachedPositionSignal>(OnFloorReachedPosition);
            _signalBus.Subscribe<GameStateChangedSignal>(OnGameStateChanged);
        }


        private void GenerateFirstFloors()
        {
            _factory.Create(new FloorView.ConstructOptions(
                new Vector2(0.0f, _config.TopFloorDefaultY),
                new Vector2(7.0f, 0.5f),
                "TopFloor"));
            
            _factory.Create(new FloorView.ConstructOptions(
                new Vector2(0.0f, _config.BottomFloorDefaultY),
                new Vector2(10.0f, 0.5f),
                "BottomFloor"));
            
        }

        private void OnFloorReachedPosition(FloorReachedPositionSignal obj)
        {
            switch (obj.PositionType)
            {
                case FloorPositionTypes.Visible:
                    GenerateNextFloor(obj.Floor);
                    break;
                case FloorPositionTypes.Finish:
                    DestroyFloor(obj.Floor);
                    break;
                default: 
                    Debug.Log("Floor position undefined");
                    break;
            }

            ;
        }

        private void OnGameStateChanged(GameStateChangedSignal obj)
        {
            switch (obj.State)
            {
                case GameStates.Restarting:
                    CleanupFloors();
                    GenerateFirstFloors();
                    Freeze();
                    break;
                case GameStates.Paused:
                    Freeze();
                    break;
                case GameStates.GameOver:
                    Freeze();
                    break;
                case GameStates.Playing:
                    Unfreeze();
                    break;
            };
        }

        private void CleanupFloors()
        {
            foreach (var floor in GameObject.FindGameObjectsWithTag("TopFloor"))
            {
                DestroyFloor(floor);
            };
            foreach (var floor in GameObject.FindGameObjectsWithTag("BottomFloor"))
            {
                DestroyFloor(floor);
            }
        }

        private void DestroyFloor(GameObject objFloor)
        {
            GameObject.Destroy(objFloor);
        }

        private void GenerateNextFloor(GameObject prevFloor)
        {
            Vector2 nextScale = new Vector2(
                Random.Range(_config.FloorScaleXMin, _config.FloorScaleXMax),
                Random.Range(_config.FloorScaleYMin, _config.FloorScaleYMax)
                );
            
            Vector2 nextPosition = new Vector2();
            float nextSpace = Random.Range(_config.FloorSpaceXMin, _config.FloorSpaceXMax);
            nextPosition.x = prevFloor.transform.position.x + prevFloor.transform.localScale.x / 2 +
                             + nextSpace + nextScale.x / 2;
            
            switch (prevFloor.tag)
            {
                case "TopFloor":
                    nextPosition.y = _config.TopFloorDefaultY;
                    break;
                case "BottomFloor":
                    nextPosition.y = _config.BottomFloorDefaultY;
                    break;
            };
            
            _factory.Create(new FloorView.ConstructOptions(
                nextPosition,
                nextScale,
                prevFloor.tag));
        }

    }
}