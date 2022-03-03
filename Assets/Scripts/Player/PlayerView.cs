using System;
using System.Collections;
using Main;
using UnityEditor.Tilemaps;
using UnityEngine;
using Zenject;

namespace Player
{
    public class PlayerView : MonoBehaviour, IDisposable
    {
        private SignalBus _signalBus;
        private PlayerStates _state;
        
        private float _timeElapsed;
        private float _sourcePosY = 0.0f;
        private float _targetPosY = 0.0f;
        private float _moveTime = 0.0f;

        public AnimationCurve moveCurve;

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
        
        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
            _state = PlayerStates.MovingToTop;

            SubscribeToSignalBus();
            SetDefaultPosition();

        }

        void Start()
        {
            _timeElapsed = Time.time;
        }

        void Update()
        {
            TryMove();
        }


        public void Dispose()
        {
            UnsubscribeFromSignalBus();
        }
        
        

        private void TryMove()
        {
            if (_state != PlayerStates.OnTop || _state != PlayerStates.OnBottom)
            {
                _timeElapsed += Time.deltaTime;
            
                switch (_state)
                {
                    case PlayerStates.MovingToTop:
                        _sourcePosY = -1.5f;
                        _targetPosY = 1.5f;
                        _moveTime = 1.0f;
                        if (transform.position.y == _targetPosY)
                        {
                            State = PlayerStates.OnTop;
                        };
                        break;
                    case PlayerStates.MovingToBottom:
                        _sourcePosY = 1.5f;
                        _targetPosY = -1.5f;
                        _moveTime = 1.0f;
                        if (transform.position.y == _targetPosY)
                        {
                            State = PlayerStates.OnBottom;
                        };
                        break;
                    case PlayerStates.FallingToTop:
                        _sourcePosY = 1.5f;
                        _targetPosY = 3f;
                        _moveTime = 0.5f;
                        if (transform.position.y == _targetPosY)
                        {
                            State = PlayerStates.FallenOnTop;
                        };
                        break;
                    case PlayerStates.FallingToBottom:
                        _sourcePosY = -1.5f;
                        _targetPosY = -3f;
                        _moveTime = 0.5f;
                        if (transform.position.y == _targetPosY)
                        {
                            State = PlayerStates.FallenOnBottom;
                        };
                        break;
                } ;

                transform.position = new Vector3(
                    transform.position.x,
                    Mathf.Lerp(_sourcePosY, _targetPosY, moveCurve.Evaluate(_timeElapsed / _moveTime)),
                    transform.position.z);
            }
            
        }
        
        private void SetDefaultPosition()
        {
            transform.position = new Vector2(-1.3f, -1.5f);
        }

        private void SubscribeToSignalBus()
        {
            _signalBus.Subscribe<PlayerMoveToBottomSignal>(OnPlayerMoveToBottom);
            _signalBus.Subscribe<PlayerMoveToTopSignal>(OnPlayerMoveToTop);
        }

        private void UnsubscribeFromSignalBus()
        {
            
            _signalBus.Unsubscribe<PlayerMoveToBottomSignal>(OnPlayerMoveToBottom);
            _signalBus.Unsubscribe<PlayerMoveToTopSignal>(OnPlayerMoveToTop);
        }

    
        
        
        private void OnPlayerMoveToBottom(PlayerMoveToBottomSignal obj)
        {
            Debug.Log("Player moving to bottom...");
            _state = PlayerStates.MovingToBottom;
        }
        
        private void OnPlayerMoveToTop(PlayerMoveToTopSignal obj)
        {
            Debug.Log("Player moving to top...");
            _state = PlayerStates.MovingToTop;
        }

        private void OnStateChanged(PlayerStates state)
        {
            _signalBus.Fire(new PlayerStateChangedSignal(state));
        }
    }
}