using System;
using Models.Animations;
using UnityEngine;

namespace Models.Invaders
{
    [Serializable]
    public class SpecialShipData
    {
        public int            value;
        public int            health;
        public Mesh           mesh;
        public Color          color;
        public Vector3        startPos;
        public Vector3        targetPos;
        public AnimationCurve moveEase;
        public float          duration;
        public float          appearanceRate;
        public TweenData      killAnimation;
    }
}