using System;
using Models.Animations;
using UnityEngine;

namespace Models.Invaders
{
    public enum InvadersLabel
    {
        Invader10, 
        Invader20, 
        Invader30
    }
    
    [Serializable]
    public struct InvaderDataSet
    {
        public InvadersLabel label;
        public int           value; 
        public Mesh[]        meshes;
    }
}