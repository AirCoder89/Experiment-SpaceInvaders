using Core;
using Models.SystemConfigs;
using UnityEngine;
using Views;

namespace Systems
{
    public class ShieldsSystem : GameSystem
    {
        private readonly Transform     _holder;
        private readonly ShieldConfig  _config;
        private ShieldView[]           _shields;
        
        public ShieldsSystem(SystemConfig inConfig = null) : base(inConfig)
        {
            if(inConfig != null) _config = inConfig as ShieldConfig;
            _holder = new GameObject("Shields Holder").transform;
            _holder.SetParent(LevelState.Instance.transform);
            _holder.position = Vector3.zero;
        }
        
        public override void Start()
        {
            Vector3 pos = _config.shieldData.startAnchor;
            _shields = new ShieldView[_config.shieldData.shieldCount];
            for (var i = 0; i < _config.shieldData.shieldCount; i++)
            {
                _shields[i] = new ShieldView($"Shield_{i}", _config);
                _shields[i].SetParent(_holder);
                _shields[i].SetPosition(pos);
                pos = new Vector3(pos.x + _shields[i].GetWidth() + _config.shieldData.shieldSpacing, pos.y, 0f);
            }
        }
    }
}