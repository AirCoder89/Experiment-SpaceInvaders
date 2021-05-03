using System;
using Systems;
using AirCoder.TJ.Core.Extensions;
using Core;
using Interfaces;
using Models.SystemConfigs;
using UnityEngine;
using Utils;

namespace Views
{
    public class PlayerView : GameView3D, IDestructible
    {
        public static event Action<PlayerView> OnDestroyed;
        public static event Action OnReviveRequest;
        
        public bool                   IsAlive { get; private set; }
        public int Health
        {
            get => _health;
            private set
            {
                _health = value;
                IsAlive = _health > 0;
                if (IsAlive) return;
                OnDestroyed?.Invoke(this);
                    Kill();
            }
        }
        private ShootingSystem _shootingSystem 
            => _shoot ?? (_shoot = Main.GetSystem<ShootingSystem>());

        private readonly PlayerConfig  _config;
        private ShootingSystem         _shoot;
        private readonly int           _startHealth;
        private string                 _blinkOperationId;
        private int                    _blinkAmount = 7;
        private int                    _health;
        
        public PlayerView(string inName,PlayerConfig inConfig) : base(inName, inConfig.mesh)
        {
            _config = inConfig;
            layerMask = LayersList.Player;
            GetComponent<MeshRenderer>().material.color = _config.color;
            SetPosition(_config.startPosition);
            _startHealth = Health = _config.health;

            GetComponent<BoxCollider>().center = new Vector3(0f, 0.05f, 0f);
            GetComponent<BoxCollider>().size = new Vector3(0.8f, 0.43f, 1f);
        }

        public void Reset()
        {
            transform.SetPositionAndRotation(_config.startPosition, Quaternion.identity);
            SetScale(Vector3.one);
            
            PlayReviveAnimation(0, () =>
            {
                Health = _startHealth;
                Visibility = true;
                gameObject.SetActive(true);
            });
        }
        
        public void TakeDamage()
        {
            if(!IsAlive) return;
            Health--;
        }

        private void PlayKillAnimation()
        {
            var animationData = _config.killAnimation;
            transform
                .TweenLocalRotation(animationData.target, animationData.duration)
                .SetEase(animationData.ease)
                .Play();
            
            transform
                .TweenScale(Vector3.zero, animationData.duration)
                .SetEase(animationData.ease)
                .OnComplete(() =>
                {
                    Visibility = false;
                    OnReviveRequest?.Invoke();
                })
                .Play();
        }

        private void PlayReviveAnimation(int counter, Action callback)
        {
            if (counter >= _blinkAmount)
            {
                Main.CancelExecution(_blinkOperationId);
                callback?.Invoke();
                return;
            }
            _blinkOperationId = Main.LateExecute((() =>
            {
                gameObject.SetActive(!gameObject.activeSelf);
                counter++;
                PlayReviveAnimation(counter, callback);
            }), 0.2f);
        }

        public int Kill()
        {
            AudioSystem.Play(AudioLabel.HitPlayer);
            PlayKillAnimation();
            return 0;
        }

        public void Move(float inHorizontalAxes)
        {
            if(!IsAlive) return;
            if(inHorizontalAxes < 0f && Position.x <= _config.movementRange.x) return; //prevent move left
            if(inHorizontalAxes > 0f && Position.x >= _config.movementRange.y) return; //prevent move right
            var translation = inHorizontalAxes * _config.speed * Time.deltaTime;
            gameObject.transform.Translate(translation, 0, 0);
        }

        public void Shoot()
        {
            if(!IsAlive) return;
            _shootingSystem.Shoot(Position, Vector3.up, _config.targetLayer, "Player");
        }

        
    }
}