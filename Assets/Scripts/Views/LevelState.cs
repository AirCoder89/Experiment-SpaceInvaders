using Core;
using Interfaces;

namespace Views
{
    public sealed class LevelState : GameView, IGameState
    {
        public static LevelState Instance { get; private set; }
        
        public LevelState(string inName) : base(inName)
        {
            if(Instance == null) Instance = this;
            else Destroy();
        }
    }
}