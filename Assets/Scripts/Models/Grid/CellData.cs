using System;
using UnityEngine;
using Vector2Int = Utils.Array2D.Vector2Int;

namespace Models.Grid
{
    [Serializable]
    public struct CellData
    {
        public int         value;
        public Color       color;
        public Mesh[]        meshes;
        public Vector2Int  position;
    }
}