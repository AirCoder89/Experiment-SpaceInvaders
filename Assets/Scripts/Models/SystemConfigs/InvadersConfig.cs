using AirCoder.NaughtyAttributes.Scripts.Core.MetaAttributes;
using Core;
using UnityEngine;

namespace Models.SystemConfigs
{
    [CreateAssetMenu(menuName = "Game/System Config/Invaders System Config")]
    public class InvadersConfig : SystemConfig
    {
        public InvadersMovementData  movement;
        public InvaderBehaviours     behaviours;

        [BoxGroup("Special Ship")] public Mesh    specialShipMesh;
        [BoxGroup("Special Ship")] public Vector3 specialShipStartPos;
        [BoxGroup("Special Ship")] public Vector3 specialShipTargetPos;
        [BoxGroup("Special Ship")] public float   specialShipSpeed;
        [BoxGroup("Special Ship")] public float   specialShipAppearanceRate;
        

    }
}