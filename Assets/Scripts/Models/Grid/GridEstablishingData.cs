using System;
using UnityEngine;
using Vector2Int = Utils.Array2D.Vector2Int;

namespace Models.Grid
{
    [Serializable]
    public struct GridEstablishingData
    {
        public Vector2Int     dimension;
        public Vector2        spacing;
        public Vector2        padding;
        public Vector2        cellSize;
        public Material       material;
    }
}