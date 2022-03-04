using System;
using Config;
using GUI;
using Level;
using Player;
using UnityEngine;
using Zenject;

namespace Main
{
    public class Game: IDisposable
    {
        private SignalBus _signalBus;
        private GameStates _state;
        private GameConfig _config;
        private Score _score;
        private float _playBeginTime;
        private IUIView _uiView;
        private ILevelController _levelController;
        private IPlayerController _playerController;

        public GameStates State
        {
            get
            {
                return _state;
            }
            set
            {
                _state = value;
                _signalBus.Fire(new GameStateChangedSignal(_state));
            }
        }
        

        public Game(
            SignalBus signalBus,
            GameConfig config,
            IUIView uiView,
            ILevelController levelController,
            IPlayerController playerController,
            Score score)
        {
            _signalBus = signalBus;
            State = GameStates.Restarting;
            _config = config;
            // _uiView = uiView;
            // _levelController = levelController;
            // _playerController = playerController;
            _score = score;

            SubscribeToSignalBus();
        }


        public void Dispose()
        {
            UnsubscribeFromSignalBus();
        }

        private void SubscribeToSignalBus()
        {
            _signalBus.Subscribe<GameStateChangeRequestedSignal>(OnGameStateChangeRequested);
            _signalBus.Subscribe<GameFlipGravitySignal>(OnGameFlipGravity);
        }
        
        private void UnsubscribeFromSignalBus()
        {
            _signalBus.Unsubscribe<GameStateChangeRequestedSignal>(OnGameStateChangeRequested);
            _signalBus.Unsubscribe<GameFlipGravitySignal>(OnGameFlipGravity);
        }

        private void OnGameFlipGravity(GameFlipGravitySignal obj)
        {
            switch (_state)
            {
                case GameStates.Restarting:
                    State = GameStates.Playing;
                    _playBeginTime = Time.time;
                    _score.Time = 0;
                    break;
                case GameStates.GameOver:
                    State = GameStates.Restarting;
                    break;
            };
        }

        private void OnGameStateChangeRequested(GameStateChangeRequestedSignal args)
        {
            switch (args.State)
            {
                case GameStates.Restarting:
                    State = GameStates.Restarting;
                    _score.Time = 0;
                    break;
                case GameStates.GameOver:
                    if (_state == GameStates.Playing)
                    {
                        AddScore();
                        State = GameStates.GameOver;
                    }
                    break;
                case GameStates.Paused:
                    if (_state == GameStates.Playing)
                    {
                        AddScore();
                        State = GameStates.Paused;
                    }
                    break;
                case GameStates.Playing:
                    if (_state == GameStates.Paused || _state == GameStates.Restarting)
                    {
                        State = GameStates.Playing;
                        _playBeginTime = Time.time;
                    }
                    break;
                default:
                    Debug.Log("Requested unhandled game state change");
                    break;
            } ;
        }

        private void AddScore()
        {
            _score.Time += Time.time - _playBeginTime;
        }
    }
}