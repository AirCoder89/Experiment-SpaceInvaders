using AirCoder.NaughtyAttributes.Scripts.Core.MetaAttributes;
using Core;
using UnityEngine;

namespace Models.SystemConfigs
{
    [CreateAssetMenu(menuName = "Game/System Config/Invaders System Config")]
    public class InvadersConfig : SystemConfig
    {
        [BoxGroup("Movement")] public float stepDelay; //delay after each step
        [BoxGroup("Movement")] public float stepLength;
        [BoxGroup("Movement")] public float stepDuration;
        [BoxGroup("Movement")] public float moveDownPacing;

        [BoxGroup("Edges")] public Vector3 edgesSize;
        [BoxGroup("Edges")] public Vector3 leftEdgePos;
        [BoxGroup("Edges")] public Vector3 rightEdgePos;

        [BoxGroup("Special Ship")] public Mesh    specialShipMesh;
        [BoxGroup("Special Ship")] public Vector3 specialShipStartPos;
        [BoxGroup("Special Ship")] public Vector3 specialShipTargetPos;
        [BoxGroup("Special Ship")] public float   specialShipSpeed;
        [BoxGroup("Special Ship")] public float   specialShipAppearanceRate;
        

    }
}