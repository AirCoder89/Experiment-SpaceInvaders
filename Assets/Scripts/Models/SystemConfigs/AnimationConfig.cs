using Core;
using UnityEngine;

namespace Models.SystemConfigs
{
    [CreateAssetMenu(menuName = "Game/System Config/Animation System Config")]
    public class AnimationConfig : SystemConfig
    {
        [Range(0f,2f)] 
        public float   duration;
        public int     meshesCount;
    }
}