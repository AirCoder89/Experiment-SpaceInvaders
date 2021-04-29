using AirCoder.TJ.Core;
using Core;
using Models.Grid;
using Models.Invaders;
using UnityEngine;
using Utils;
using Vector2Int = Utils.Array2D.Vector2Int;

namespace Models.SystemConfigs
{
    [CreateAssetMenu(menuName = "Game/System Config/Grid System Config")]
    public class GridConfig : SystemConfig
    {
        public GridEstablishingData  establishing;
        public GridAnimationData     animation;
        public InvaderCatalog        invaders;
    }
}