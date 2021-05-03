using AirCoder.NaughtyAttributes.Scripts.Core.MetaAttributes;
using Core;
using Models.Shields;
using UnityEngine;
using Views;
using Vector2Int = Utils.Array2D.Vector2Int;

namespace Models.SystemConfigs
{
    [CreateAssetMenu(menuName = "Game/System Config/Shields System Config")]
    public class ShieldConfig : SystemConfig
    {
        public PieceData     pieceData;
        public ShieldData    shieldData;
    }
}