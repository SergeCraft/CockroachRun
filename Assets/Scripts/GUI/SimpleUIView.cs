using System;
using Main;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace GUI
{
    
    public class SimpleUIView : MonoBehaviour, IDisposable
    {
        private SignalBus _signalBus;
        
        public Button PauseButton;
        public Button FlipButton;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;

            SubscribeToSignalBus();
        }


        public void PauseButtonClicked()
        {
            Debug.Log($"Pause Button Clicked");
            _signalBus.Fire(new GamePausedSignal(true));
            PauseButton.GetComponentInChildren<Text>().text = "Play";
        }

        public void FlipButtonClicked()
        {
            Debug.Log($"Flip Button Clicked");
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<GamePausedSignal>(OnGamePaused);
        }
        
        
        private void SubscribeToSignalBus()
        {
            _signalBus.Subscribe<GamePausedSignal>(OnGamePaused);
        }

        private void OnGamePaused(GamePausedSignal args)
        {
            if (!args.IsPaused) PauseButton.GetComponentInChildren<Text>().text = "Pause";
        }
    }
}