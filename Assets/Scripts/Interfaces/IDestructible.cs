using System;
using Core;

namespace Interfaces
{
    /// <summary>
    /// Since we have a static (predictable) game object in the scene.
    /// so we don't need to physically destroying/de-spawn the object.
    /// </summary>
    /// 
    public interface IDestructible
    {
        bool IsAlive { get; }
        int Health { get; }

        void TakeDamage();
        int Kill();
    }
}