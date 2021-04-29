using Core;
using UnityEngine;

namespace Models.SystemConfigs
{
    [CreateAssetMenu(menuName = "Game/System Config/Shooting System Config")]
    public class ShootingConfig : SystemConfig
    {
        public BulletData  bullet;
        public int         bufferSize;
    }
}