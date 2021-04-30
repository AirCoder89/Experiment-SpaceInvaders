using Core;
using Models.Invaders;
using UnityEngine;

namespace Models.SystemConfigs
{
    [CreateAssetMenu(menuName = "Game/System Config/Invaders System Config")]
    public class InvadersConfig : SystemConfig
    {
        public InvadersMovementData  movement;
        public InvaderBehaviours     behaviours;
        public SpecialShipData       specialShip;
    }
}