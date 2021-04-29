using Core;
using Interfaces;
using Models.SystemConfigs;
using UnityEngine;
using Views;

namespace Systems
{
    public class PlayerSystem : GameSystem, ITick
    {
        private InputsSystem _InputsSystem 
            => _input ?? (_input = Main.GetSystem<InputsSystem>());
        
        private PlayerConfig  _config;
        private PlayerView    _playerView;
        private InputsSystem  _input;

        public PlayerSystem(SystemConfig inConfig) : base(inConfig)
        {
            if(inConfig != null) _config = inConfig as PlayerConfig;
            _playerView = new PlayerView("Player", _config);
        }
        
        public override void Start()
        {
            
        }

        public void Tick(float inDeltaTime)
        {
            if(!IsRun) return;
            
            //if (_InputsSystem.GetButton(InputBehaviour.Shoot)) _playerView.Jump(ButtonState.Button);
            
            var horizontal = Input.GetAxis("Horizontal");
            _playerView.Move(horizontal, inDeltaTime);
            if(Input.GetKeyDown(KeyCode.Space)) _playerView.Shoot();
            
            //_playerView?.Tick(inDeltaTime);
        }
        
    }
}