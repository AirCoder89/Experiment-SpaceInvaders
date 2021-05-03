using System;
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
        private readonly PlayerConfig   _config;
        private readonly PlayerView     _playerView;
        private InputsSystem            _input;
        private bool                    _canShoot;
        private float                   _fireRateCounter;
        
        public PlayerSystem(SystemConfig inConfig) : base(inConfig)
        {
            if(inConfig != null) _config = inConfig as PlayerConfig;
            _playerView = new PlayerView("Player", _config);
            _playerView.SetParent(GameState.GameHolder);
            _fireRateCounter = 0;
            _canShoot = true;

            InputsSystem.OnMove += MovePlayer;
            InputsSystem.OnShoot += Shoot;
        }

        public void Reset()
        {
            _playerView.Reset();
            _fireRateCounter = 0;
            _canShoot = true;
        }
        
        public override void Start()
        {
            _canShoot = true;
        }

        public void Tick(float inDeltaTime)
        {
            if(!IsRun) return;
            EvaluateFireRate(inDeltaTime);
        }

        private void MovePlayer(float inHorizontal)
        {
            _playerView.Move(inHorizontal);
        }
        
        private void EvaluateFireRate(float inDeltaTime)
        {
            if(_canShoot) return;
            _fireRateCounter += inDeltaTime;
            if (!(_fireRateCounter >= _config.fireRate)) return;
                _fireRateCounter = 0f;
                _canShoot = true;
        }

        private void Shoot()
        {
            if (!_canShoot) return;
            _canShoot = false;
            _playerView.Shoot();
        }
    }
}