using System;
using AirCoder.TJ.Core;
using UnityEngine;

namespace Models.Animations
{
    [Serializable]
    public struct TweenData
    {
        public Vector3   target;
        public float     duration;
        public EaseType  ease;
    }
}