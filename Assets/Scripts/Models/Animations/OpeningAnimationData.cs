using System;
using AirCoder.TJ.Core;
using UnityEngine;
using Utils;

namespace Models.Animations
{
    [Serializable]
    public struct OpeningAnimationData
    {
        public TweenDirection     direction;
        public TweenAxis          axis;
        [Range(0,1)] public float speed;
        public EaseType           ease;
    }
}