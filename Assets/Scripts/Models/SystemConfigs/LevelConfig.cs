using Core;
using UnityEngine;

namespace Models.SystemConfigs
{
    [CreateAssetMenu(menuName = "Game/System Config/Level System Config")]
    public class LevelConfig : SystemConfig
    {
        public EdgesData  topEdge;
        public EdgesData  bottomEdge;
        public EdgesData  leftEdge;
        public EdgesData  rightEdge;
    }
}