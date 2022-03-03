using Player;

namespace Main
{
    public class GameEvents
    {
        
    }

    
    public class ComponentsLoadedSignal
    {
        
    } 
    
    
    public class GameStartedSignal
    {
        
    }
    
    
    public class GameoverSignal
    {
        
    }

    public class GamePausedSignal
    {
        public bool IsPaused { get; private set; }

        public GamePausedSignal(bool isPaused)
        {
            IsPaused = isPaused;    
        }
    }

    public class GameFlipGravitySignal
    {
        
    }
    
    public class PlayerMoveToTopSignal
    {
        
    }
    
    
    public class PlayerMoveToBottomSignal
    {
        
    }

    public class PlayerStateChangedSignal
    {
        public PlayerStates State { get; private set; }

        public PlayerStateChangedSignal(PlayerStates state)
        {
            State = state;
        }
    }
}