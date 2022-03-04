using Level;
using Player;
using UnityEngine;

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
    
    public class PlayerStateChangedSignal
    {
        public PlayerStates State { get; private set; }

        public PlayerStateChangedSignal(PlayerStates state)
        {
            State = state;
        }
    }

    public class PlayerHitSignal
    {
        public GameObject Other { get; private set; }

        public PlayerHitSignal(GameObject other)
        {
            Other = other;  
        }
    }

    public class FloorReachedPositionSignal
    {
        public GameObject Floor { get; private set; }
        public FloorPositionTypes PositionType { get; private set; }

        public FloorReachedPositionSignal(GameObject floor, FloorPositionTypes posType)
        {
            Floor = floor;
            PositionType = posType;
        }
    }
}