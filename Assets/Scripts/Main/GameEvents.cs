using Level;
using Player;
using UnityEngine;

namespace Main
{
   

    public class GameStateChangedSignal
    {
        public GameStates State { get; private set; }

        public GameStateChangedSignal(GameStates state)
        {
            State = state;
        }
    }

    public class GameStateChangeRequestedSignal
    {
        public GameStates State { get; private set; }

        public GameStateChangeRequestedSignal(GameStates state)
        {
            State = state;
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
    
    public class PlayerLeftSignal
    {
        public GameObject Other { get; private set; }

        public PlayerLeftSignal(GameObject other)
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