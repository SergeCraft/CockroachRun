using System;
using Config;
using Main;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Level
{
    public class SimpleLevelController : ITickable, IDisposable
    {
        private FloorView.Factory _factory;
        private SignalBus _signalBus;
        private GameConfig _config;

        public SimpleLevelController(FloorView.Factory factory, SignalBus signalBus, GameConfig config)
        {
            _factory = factory;
            _signalBus = signalBus;
            _config = config;

            _factory.Create(new FloorView.ConstructOptions(
                new Vector2(10.0f, -3.0f),
                new Vector2(3.0f, 0.5f),
                "BottomFloor"));
            
            SubscribeToSignalBus();
            GenerateFirstFloors();
        }

        public void Tick()
        {
            
        }


        public void Dispose()
        {
            UnsubscribeFromSignalBus();
        }

        private void UnsubscribeFromSignalBus()
        {
            _signalBus.Unsubscribe<FloorReachedPositionSignal>(OnFloorReachedPosition);
        }

        private void SubscribeToSignalBus()
        {
            _signalBus.Subscribe<FloorReachedPositionSignal>(OnFloorReachedPosition);
        }

        private void GenerateFirstFloors()
        {
            _factory.Create(new FloorView.ConstructOptions(
                new Vector2(0.0f, 3.0f),
                new Vector2(7.0f, 0.5f),
                "TopFloor"));
            
            _factory.Create(new FloorView.ConstructOptions(
                new Vector2(0.0f, -3.0f),
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

        private void DestroyFloor(GameObject objFloor)
        {
            GameObject.Destroy(objFloor);
        }

        private void GenerateNextFloor(GameObject prevFloor)
        {
            Vector2 nextScale = new Vector2(
                Random.Range(1.0f, 10.0f),
                0.5f
                );
            
            Vector2 nextPosition = new Vector2();
            float nextSpace = Random.Range(1.0f, 5.0f);
            nextPosition.x = prevFloor.transform.position.x + prevFloor.transform.localScale.x / 2 +
                             +nextSpace + nextScale.x / 2;
            
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