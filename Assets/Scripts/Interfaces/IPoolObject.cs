using System;
using Core;

namespace Interfaces
{
    public interface IPoolObject
    {
        event Action<Type, GameView> onDspawn;
        void Despawn();
    }
}