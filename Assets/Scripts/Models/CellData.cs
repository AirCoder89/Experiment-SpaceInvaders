using System;
using UnityEngine;
using Vector2Int = Utils.Array2D.Vector2Int;

namespace Models
{
    [Serializable]
    public struct CellData
    {
        public int value;
        public Color color;
        public Mesh mesh;
        public Vector2Int position;
        
    }
}