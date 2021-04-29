using UnityEngine;

namespace Utils
{
    public static class LayersList
    {
        public static LayerMask Player => LayerMask.NameToLayer("Player");
        public static LayerMask Invaders => LayerMask.NameToLayer("Invaders"); 
        public static LayerMask Shields => LayerMask.NameToLayer("Shield");
        public static LayerMask Other => LayerMask.NameToLayer("Other");
    }
}