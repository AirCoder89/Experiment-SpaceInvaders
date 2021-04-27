using AirCoder.TJ.Core;
using Core;
using Models.Invaders;
using UnityEngine;
using Utils;
using Vector2Int = Utils.Array2D.Vector2Int;

namespace Models.SystemConfigs
{
    [CreateAssetMenu(menuName = "Game/System Config/Grid System Config")]
    public class GridConfig : SystemConfig
    {
        public TweenDirection tweenDirection;
        public TweenAxis tweenAxis;
        [Range(0,1)] public float tweenSpeed;
        public EaseType tweenEase;

        public float invaderStep;
        public float invaderStepDuration;
        
        public Vector2Int dimension;
        public Vector2 spacing;
        public Vector2 padding;
        public Vector2 cellSize;
        public Material material;
        public InvaderCatalog invaders;
    }
}