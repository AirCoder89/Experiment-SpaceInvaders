using AirCoder.NaughtyAttributes.Scripts.Core.MetaAttributes;
using Core;
using UnityEngine;
using Vector2Int = Utils.Array2D.Vector2Int;

namespace Models.SystemConfigs
{
    [CreateAssetMenu(menuName = "Game/System Config/Shields System Config")]
    public class ShieldConfig : SystemConfig
    {
        [BoxGroup("Pieces")] public Mesh       meshPiece;
        [BoxGroup("Pieces")] public Vector3    pieceScale;
        [BoxGroup("Pieces")] public float      spacing;
        [BoxGroup("Pieces")] public LayerMask  layerMask;

        [BoxGroup("Shield")] public int        shieldAmount = 3;
        [BoxGroup("Pieces")] public Vector2Int shieldDimension;
        [BoxGroup("Shield")] public Vector2    startAnchor;
        [BoxGroup("Shield")] public float      shieldSpacing;
    }
}