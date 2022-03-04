using System;
using Main;
using UnityEngine;
using Zenject;

namespace Player
{
    public class SimplePlayerController: IPlayerController, IDisposable
    {
        private SignalBus _signalBus;
        private PlayerStates _state;
        private PlayerStates _prevState;
        private GameStates _gameState;
        private bool _isGravityFlipped;
        private DiContainer _container;
        
        
        public PlayerStates State
        {
            get
            {
                return _state;
            }
            private set
            {
                _state = value;
                OnStateChanged(_state);
            }
        }


        public SimplePlayerController(SignalBus signalBus)
        {
            _signalBus = signalBus;
            _state = PlayerStates.Restarting;
            _isGravityFlipped = false;

            SubscribeToSignalBus();
        }



        public void Dispose()
        {
            UnsubscribeFromSignalBus();
        }
        
        
        private void SubscribeToSignalBus()
        {
            _signalBus.Subscribe<PlayerHitSignal>(OnPlayerHit);
            _signalBus.Subscribe<PlayerLeftSignal>(OnPlayerLeft);
            _signalBus.Subscribe<GameFlipGravitySignal>(OnGravityFlip);
            _signalBus.Subscribe<GameStateChangedSignal>(OnGameStateChanged);
        }



        private void UnsubscribeFromSignalBus()
        {
            _signalBus.Unsubscribe<PlayerHitSignal>(OnPlayerHit);
            _signalBus.Unsubscribe<PlayerLeftSignal>(OnPlayerLeft);
            _signalBus.Unsubscribe<GameFlipGravitySignal>(OnGravityFlip);
            _signalBus.Unsubscribe<GameStateChangedSignal>(OnGameStateChanged);
        }
        
        
        private void OnStateChanged(PlayerStates state)
        {
            _signalBus.Fire(new PlayerStateChangedSignal(state));
        }
        
        private void OnGameStateChanged(GameStateChangedSignal args)
        {
            switch (args.State)
            {
                case GameStates.Paused:
                    _prevState = _state;
                    State = PlayerStates.Frozen;
                    break;
                case GameStates.Playing:
                    if (_state == PlayerStates.Frozen)
                    {
                        State = PlayerStates.Unfrozen;
                        State = _prevState;
                    }
                    break;
                case GameStates.Restarting:
                    State = PlayerStates.Restarting;
                    break;
            };
            
            _gameState = args.State;
            
        }
        
        private void OnPlayerHit(PlayerHitSignal obj)
        {
            switch (obj.Other.tag)
            {
                case "TopFloor":
                    _state = PlayerStates.OnTop;
                    break;
                case "BottomFloor":
                    _state = PlayerStates.OnBottom;
                    break;
                case "FallZone":
                    _state = PlayerStates.Fallen;
                    _signalBus.Fire(new GameStateChangeRequestedSignal(GameStates.GameOver));
                    break;
                default:
                    Debug.Log($"Player hit undefined object with tag {obj.Other.tag}");
                    break;
            };
        }
        
        private void OnPlayerLeft(PlayerLeftSignal obj)
        {
            if (_state == PlayerStates.OnTop || _state == PlayerStates.OnBottom)
            {
                switch (obj.Other.tag)
                {
                    case "TopFloor":
                        State = PlayerStates.MovingToTop;
                        break;
                    case "BottomFloor":
                        State = PlayerStates.MovingToBottom;
                        break;
                    default:
                        Debug.Log($"Player left undefined object with tag {obj.Other.tag}");
                        break;
                };
            }
        }

        private void OnGravityFlip()
        {
            if (_gameState == GameStates.Playing)
            {
                if (_state == PlayerStates.OnBottom)
                {
                    _isGravityFlipped = !_isGravityFlipped;
                    State = PlayerStates.MovingToTop;
                }
                else if (_state == PlayerStates.OnTop)
                {
                    _isGravityFlipped = !_isGravityFlipped;
                    State = PlayerStates.MovingToBottom;
                };
            }
        }

    }
}