using System;
using UnityEngine;

namespace Models
{
    [Serializable]
    public struct BulletData
    {
        public float    speed;
        public Sprite   sprite;
        public Vector3  colliderSize;
        public Vector2  scale;
    }
}