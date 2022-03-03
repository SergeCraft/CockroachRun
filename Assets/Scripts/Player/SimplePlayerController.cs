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
        private bool _isGravityFlipped;
        
        
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
            _state = PlayerStates.OnBottom;
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
            _signalBus.Subscribe<GameFlipGravitySignal>(OnGravityFlip);
        }


        private void UnsubscribeFromSignalBus()
        {
            _signalBus.Unsubscribe<PlayerHitSignal>(OnPlayerHit);
            _signalBus.Unsubscribe<GameFlipGravitySignal>(OnGravityFlip);
        }
        
        
        private void OnStateChanged(PlayerStates state)
        {
            _signalBus.Fire(new PlayerStateChangedSignal(state));
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
                    _signalBus.Fire<GameoverSignal>();
                    break;
                default:
                    Debug.Log($"Player hit undefined object with tag {obj.Other.tag}");
                    break;
            };
        }

        private void OnGravityFlip()
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