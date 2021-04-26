using Core;

namespace Interfaces
{
    public interface IComponent
    {
        GameView View { get; }
        void Destroy();
    }
}