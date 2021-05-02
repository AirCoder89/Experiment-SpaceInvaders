using Core;

namespace Interfaces
{
    public interface IGameState
    {
        States Label { get; }
        bool Visibility { get; }
        
        void Enter();
        void Tick(float inDeltaTime);
        void Exit();
    }
}