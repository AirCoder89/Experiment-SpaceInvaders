using System;
using UnityEngine;

namespace Models.Shields
{
    [Serializable]
    public struct PieceData
    {
        public int        shieldHealth;
        public Mesh       meshPiece;
        public Vector3    pieceScale;
        public float      spacing;
    }
}