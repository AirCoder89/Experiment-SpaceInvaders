using UnityEngine;

namespace Interfaces
{
    public interface IShooter
    {
        LayerMask TargetLayer { get; }
        void Shoot();
    }
}