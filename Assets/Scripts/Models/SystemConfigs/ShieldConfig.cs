using Core;
using Models.Shields;
using UnityEngine;

namespace Models.SystemConfigs
{
    [CreateAssetMenu(menuName = "Game/System Config/Shields System Config")]
    public class ShieldConfig : SystemConfig
    {
        public PieceData     pieceData;
        public ShieldData    shieldData;
    }
}