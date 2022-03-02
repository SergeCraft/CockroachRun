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