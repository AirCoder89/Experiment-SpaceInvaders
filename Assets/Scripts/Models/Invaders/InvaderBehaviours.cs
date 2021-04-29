using System;
using UnityEngine;

namespace Models.Invaders
{
    [Serializable]
    public struct InvaderBehaviours
    {
        public LayerMask targetLayer;
        public float shootingRate;
    }
}