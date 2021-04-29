using System;
using Systems;
using Core;
using Interfaces;
using Models.SystemConfigs;
using UnityEngine;
using Utils;

namespace Views
{
    public class PlayerView : GameView3D, IDestructible
    {
        public event Action<GameView> onDestroyed;
        public bool                   IsAlive { get; private set; }
        public int Health
        {
            get => _health;
            private set
            {
                _health = value;
                IsAlive = _health > 0;
                if (IsAlive) return;
                //kill
                onDestroyed?.Invoke(this);
                Visibility = false;
                //Todo: add sfx
            }
        }
        private ShootingSystem _shootingSystem 
            => _shoot ?? (_shoot = Main.GetSystem<ShootingSystem>());

        private readonly PlayerConfig  _config;
        private readonly int           _startHealth;
        private ShootingSystem         _shoot;
        private int                    _health;
        public PlayerView(string inName,PlayerConfig inConfig) : base(inName, inConfig.mesh, LayersList.Player)
        {
            _config = inConfig;
            GetComponent<MeshRenderer>().material.color = _config.color;
            SetPosition(_config.startPosition);
            _startHealth = Health = _config.health;

            GetComponent<BoxCollider>().center = new Vector3(0f, 0.05f, 0f);
            GetComponent<BoxCollider>().size = new Vector3(0.8f, 0.43f, 1f);
        }

        public void TakeDamage()
        {
            if(!IsAlive) return;
            Health--;
        }

        public void Revive()
        {
            Health = _startHealth;
            Visibility = true;
        }

        public void Move(float inHorizontalAxes, float inDeltaTime)
        {
            if(inHorizontalAxes < 0f && Position.x <= _config.movementRange.x) return; //prevent move left
            if(inHorizontalAxes > 0f && Position.x >= _config.movementRange.y) return; //prevent move right
            var translation = inHorizontalAxes * _config.speed * inDeltaTime;
            gameObject.transform.Translate(translation, 0, 0);
        }

        public void Shoot()
        {
            _shootingSystem.Shoot(Position, Vector3.up, _config.targetLayer);
        }
    }
}