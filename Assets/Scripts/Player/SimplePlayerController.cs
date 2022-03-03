using System;
using Main;
using Zenject;

namespace Player
{
    public class SimplePlayerController: IPlayerController, IDisposable
    {
        private SignalBus _signalBus;
        private PlayerStates _state;
        
        
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
            _state = PlayerStates.MovingToTop;

            SubscribeToSignalBus();

            Test();
        }



        public void Dispose()
        {
            UnsubscribeFromSignalBus();
        }
        
        
        private void Test()
        {
            _signalBus.Fire<PlayerMoveToTopSignal>();
        }
        
        private void SubscribeToSignalBus()
        {
            ;
        }
        
        private void UnsubscribeFromSignalBus()
        {
            ;
        }
        
        
        private void OnStateChanged(PlayerStates state)
        {
            _signalBus.Fire(new PlayerStateChangedSignal(state));
        }
    }
}