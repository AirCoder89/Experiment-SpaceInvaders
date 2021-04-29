using System;
using UnityEngine;

namespace Models
{
    [Serializable]
    public struct InvaderBehaviours
    {
        public LayerMask targetLayer;
        public float shootingRate;
    }
}