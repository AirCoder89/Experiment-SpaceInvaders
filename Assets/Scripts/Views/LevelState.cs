using Core;
using Interfaces;

namespace Views
{
    public sealed class LevelState : GameView, IGameState
    {
        public GameStates Label { get; }
        public static LevelState Instance { get; private set; }
        
        public LevelState(string inName,GameStates inLabel) : base(inName)
        {
            if (Instance == null)
            {
                Instance = this;
                Label = inLabel;
                Main.RegisterGameState(this);
            }
            else Destroy();
        }

        
    }
}