using System;
using UnityEngine;
using Vector2Int = Utils.Array2D.Vector2Int;

namespace Models
{
    [Serializable]
    public struct ShieldData
    {
        public int        shieldCount;
        public Vector2Int shieldDimension;
        public Vector2    startAnchor;
        public float      shieldSpacing;
    }
}