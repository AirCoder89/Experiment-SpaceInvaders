using System;
using UnityEngine;

namespace Models.Invaders
{
    [Serializable]
    public class SpecialShipData
    {
        public Mesh     mesh;
        public Color    color;
        public Vector3  startPos;
        public Vector3  targetPos;
        public float    speed;
        public float    appearanceRate;
    }
}