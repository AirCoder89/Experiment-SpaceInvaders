using System;
using UnityEngine;

namespace Models.Invaders
{
    [Serializable]
    public struct InvadersMovementData
    {
        [Range(0f,2f)] public float   stepDelay; //delay after each step
        [Range(0f,2f)] public float   stepLength;
        [Range(0f,2f)] public float   stepDuration;
        [Range(0f,2f)] public float   moveDownPacing;
    }
}