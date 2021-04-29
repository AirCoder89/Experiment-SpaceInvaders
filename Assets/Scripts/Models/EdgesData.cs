using System;
using UnityEngine;

namespace Models
{
    [Serializable]
    public struct EdgesData
    {
        public LayerMask  targetLayer;
        public Vector3    size;
        public Vector3    position;
    }
}