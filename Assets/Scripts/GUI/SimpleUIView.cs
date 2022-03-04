using System;
using Config;
using Main;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using Zenject;

namespace GUI
{
    
    public class SimpleUIView : MonoBehaviour, IDisposable, IUIView
    {
        private SignalBus _signalBus;
        private GameStates _gameState;
        private Score _score;
        
        public Button PauseButton;
        public Button FlipButton;
        public Text Info;

        [Inject]
        public void Construct(SignalBus signalBus, Score score)
        {
            _signalBus = signalBus;
            _score = score;

            SubscribeToSignalBus();
        }


        public void PauseButtonClicked()
        {
            Debug.Log($"Pause Button Clicked");
            switch (_gameState)
            {
                case GameStates.Paused:
                    _signalBus.Fire(new GameStateChangeRequestedSignal(GameStates.Playing));
                    PauseButton.GetComponentInChildren<Text>().text = "Pause";
                    break;
                case GameStates.Playing:
                    _signalBus.Fire(new GameStateChangeRequestedSignal(GameStates.Paused));
                    PauseButton.GetComponentInChildren<Text>().text = "Play";
                    break;
                default:
                    break;
            }
            
        }

        public void FlipButtonClicked()
        {
            Debug.Log($"Flip Button Clicked");
            _signalBus.Fire<GameFlipGravitySignal>();
        }
      
        public void Dispose()
        {
            _signalBus.Unsubscribe<GameStateChangedSignal>(OnGameStateChanged);
        }
        
        
        private void SubscribeToSignalBus()
        {
            _signalBus.Subscribe<GameStateChangedSignal>(OnGameStateChanged);
        }

        private void OnGameStateChanged(GameStateChangedSignal args)
        {
            _gameState = args.State;
            
            switch (args.State)
            {
                case GameStates.Paused:
                    PauseButton.GetComponentInChildren<Text>().text = "Play";
                    break;
                
                case GameStates.Playing:
                    Helper.ToggleVisibleButton( 
                        PauseButton, 
                        out PauseButton, 
                        true,
                        false,
                        "Pause");
                    Helper.ToggleVisibleButton( 
                        FlipButton, 
                        out FlipButton, 
                        true,
                        true,
                        "");
                    Info.text = "";
                    break;
                
                case GameStates.GameOver:
                    Info.text = $"Game over\nTotal time: {Mathf.Round(_score.Time)}";
                    break;
                
                case GameStates.Restarting:
                    Helper.ToggleVisibleButton( 
                        PauseButton,
                        out PauseButton,
                        false);
                    
                    break;
                    
            }; 
        }
    }
}