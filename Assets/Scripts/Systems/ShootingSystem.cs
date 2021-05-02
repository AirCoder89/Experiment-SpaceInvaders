using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Interfaces;
using Models.SystemConfigs;
using UnityEngine;
using Utils;
using Views;

namespace Systems
{
    public class ShootingSystem : GameSystem, ITick
    {
        private HashSet<GameView> _bulletsInScene;
        private ShootingConfig      _config;
        private Transform           _holder;
       
        public ShootingSystem(SystemConfig inConfig) : base(inConfig)
        {
            if(inConfig != null) _config = inConfig as ShootingConfig;
            
            _holder = new GameObject("Bullets Holder").transform;
            _holder.SetParent(GameState.GameHolder);
            _holder.SetPositionAndRotation(Vector3.zero, Quaternion.identity);

            _bulletsInScene = new HashSet<GameView>();
        }
        
        public override void Start()
        {
            //Fill the pool
            for (var i = 0; i < _config.bufferSize; i++)
                ObjectPool.AddToPool<BulletView>(new BulletView("Bullet", _config.bullet, _holder));
        }

        public void Shoot(Vector3 inPosition, Vector3 inDirection, LayerMask inLayerMask, string by)
        {
            
            var bullet = ObjectPool.GetObject<BulletView>();
            if (bullet == null)
                GameExceptions.Exception($"Pool is Empty! increase the size of the buffer.");
            
            AudioSystem.Play(AudioLabel.Shoot);
            bullet.Launch(inPosition, inDirection, inLayerMask, by);
            bullet.onDspawn += OnDespawnBullet;
            _bulletsInScene.Add(bullet);
        }

        private void OnDespawnBullet(Type inType, GameView inBullet)
        {
            _bulletsInScene.Remove(inBullet);
        }

        public void Tick(float inDeltaTime)
        {
            if(!IsRun) return;
            foreach (var bullet in _bulletsInScene.ToList())
                ((BulletView)bullet).Tick(inDeltaTime);
        }
    }
}