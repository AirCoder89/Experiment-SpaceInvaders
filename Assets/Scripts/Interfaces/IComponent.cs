using System;
using Core;

namespace Interfaces
{
    public delegate void IComponentEvents(Type inType, IComponent inComponent);
    
    public interface IComponent
    {
        string Id { get; }
        event  IComponentEvents onDestroyed;
        GameView View { get; }
        void Attach(GameView inView);
        void Destroy();
        
    }
}