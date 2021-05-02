using Core;
using Models.Animations;
using UnityEngine;

namespace Models.SystemConfigs
{
    [CreateAssetMenu(menuName = "Game/System Config/Player System Config")]
    public class PlayerConfig : SystemConfig
    {
        [Range(0f,20f)] public float  speed;
        [Range(0f,2f)]  public float  fireRate;
        
        public int       health;
        public int       lives;
        public Mesh      mesh;
        public Color     color;
        public Vector3   startPosition;
        public Vector2   movementRange;
        public LayerMask targetLayer;
        public TweenData killAnimation;
    }
}