using System;

namespace Models
{
    [Serializable]
    public struct InvadersMovementData
    {
        public float   stepDelay; //delay after each step
        public float   stepLength;
        public float   stepDuration;
        public float   moveDownPacing;
    }
}