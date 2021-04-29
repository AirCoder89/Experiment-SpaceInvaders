using System;
using UnityEngine;
using Vector2Int = Utils.Array2D.Vector2Int;

namespace Models.Shields
{
    [Serializable]
    public struct ShieldData
    {
        public int        shieldCount;
        public Vector2Int shieldDimension;
        public Vector2    startAnchor;
        [Range(0f,2f)] 
        public float      shieldSpacing;
    }
}