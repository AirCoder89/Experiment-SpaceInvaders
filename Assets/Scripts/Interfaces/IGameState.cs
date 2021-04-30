using Core;

namespace Interfaces
{
    public interface IGameState
    {
        GameStates Label { get; }
        bool Visibility { get; set; }
    }
}