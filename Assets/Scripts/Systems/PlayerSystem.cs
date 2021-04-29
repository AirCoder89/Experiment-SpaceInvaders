using Core;
using Interfaces;
using Models.SystemConfigs;
using UnityEngine;
using Utils;
using Views;

namespace Systems
{
    public class PlayerSystem : GameSystem, ITick
    {
        private InputsSystem _InputsSystem 
            => _input ?? (_input = Main.GetSystem<InputsSystem>());
        
        private readonly PlayerConfig   _config;
        private readonly PlayerView     _playerView;
        private readonly Timer          _fireRateTimer;
        private InputsSystem            _input;
        private bool                    _canShoot;
        
        public PlayerSystem(SystemConfig inConfig) : base(inConfig)
        {
            if(inConfig != null) _config = inConfig as PlayerConfig;
            _playerView = new PlayerView("Player", _config);
            _playerView.SetParent(LevelState.Instance.transform);
            _fireRateTimer = new Timer(_config.fireRate, () => _canShoot = true);
        }
        
        public override void Start()
        {
            _canShoot = true;
        }

        public void Tick(float inDeltaTime)
        {
            if(!IsRun) return;
            var horizontal = Input.GetAxis("Horizontal");
            _playerView.Move(horizontal, inDeltaTime);
            if(Input.GetKeyDown(KeyCode.Space)) Shoot();
        }

        private void Shoot()
        {
            if(!_canShoot) return;
            _canShoot = false;
            _fireRateTimer.Start();
            _playerView.Shoot();
        }
    }
}