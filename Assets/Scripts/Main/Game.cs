using System;
using Config;
using UnityEngine;
using Zenject;

namespace Main
{
    public class Game: IDisposable
    {
        private SignalBus _signalBus;
        private GameStates _state;
        private GameConfig _config;

        public Game(SignalBus signalBus, IConfigManager configMgr)
        {
            _signalBus = signalBus;
            _state = GameStates.Paused;
            _config = configMgr.GetConfig();

            SubscribeToSignalBus();
        }


        public void Dispose()
        {
            UnsubscribeFromSignalBus();
        }

        private void SubscribeToSignalBus()
        {
            _signalBus.Subscribe<ComponentsLoadedSignal>(OnComponentsLoaded);
            _signalBus.Subscribe<GameStartedSignal>(OnGameStarted);
            _signalBus.Subscribe<GameoverSignal>(OnGameover);
        }
        
        private void UnsubscribeFromSignalBus()
        {
            
            _signalBus.Unsubscribe<ComponentsLoadedSignal>(OnComponentsLoaded);
            _signalBus.Unsubscribe<GameStartedSignal>(OnGameStarted);
            _signalBus.Unsubscribe<GameoverSignal>(OnGameover);
        }

        private void OnComponentsLoaded(ComponentsLoadedSignal args)
        {
            ShowUI();
        }

        private void ShowUI()
        {
            Debug.Log("Showing UI...");
        }

        private void HideUI()
        {
            Debug.Log("Hiding UI...");
        }

        private void OnGameStarted(GameStartedSignal args)
        {
            _state = GameStates.Playing;
            HideUI();
        }

        private void OnGameover(GameoverSignal args)
        {
            _state = GameStates.GameOver;
            ShowUI();
        }
    }
}