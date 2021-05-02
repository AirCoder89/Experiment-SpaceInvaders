using System;
using Systems;
using AirCoder.TJ.Core.Extensions;
using Core;
using Interfaces;
using Models.Invaders;
using Models.SystemConfigs;
using UnityEngine;
using Utils;

namespace Views
{
    public enum ShipState
    {
        None, Wait, Move
    }
    public class SpecialShip : GameView3D, IDestructible
    {
        public static event Action<int> OnDestroyed;
        public bool IsAlive { get; private set; }
        
        public int  Health
        {
            get => _health;
            private set
            {
                _health = value;
                IsAlive = _health > 0;
                if (IsAlive) return;
                OnDestroyed?.Invoke(CurrentScore);
                Kill();
            }
        }

        private int CurrentScore
        {
            get
            {
                var ratio = Mathf.Clamp(Mathf.Sin(_positionRatio * Mathf.PI), 0f, 1f);
                return Mathf.RoundToInt(_data.value * ratio);
            }
        }

        private readonly SpecialShipData _data;
        private ShipState                _state;
        private string                   _waitingOperationId;
        private float                    _movementTimeCounter;
        private float                    _positionRatio;
        private int                      _health;
        
        public SpecialShip(string inName, SpecialShipData inData)  : base(inName, inData.mesh)
        {
            _state = ShipState.None;
            _data = inData;
            Health = _data.health;
            layerMask = LayersList.Special;
            GetComponent<MeshRenderer>().material.color = _data.color;
            GetComponent<BoxCollider>().center = new Vector3(0f, 0.2f, 0f);
            GetComponent<BoxCollider>().size = new Vector3(1.3f, 0.6f, 1f);
        }

        public void Reset()
        {
            AudioSystem.Stop(AudioLabel.SpecialMove);
            Main.CancelExecution(_waitingOperationId);
            _state = ShipState.None;
            Health = _data.health; 
            Visibility = true;
            SetPosition(_data.startPos);
            SetScale(Vector3.one);
        }
            
        public void Tick(float inDeltaTime)
        {
            if(!IsAlive) return;
            switch (_state)
            {
                case ShipState.Wait: return;
                case ShipState.None: Wait(); break;
                case ShipState.Move: Move(inDeltaTime); break;
            }
        }

        private void Wait()
        {
            _state = ShipState.Wait;
            _waitingOperationId = Main.LateExecute(() =>
            {
                _movementTimeCounter = 0f;
                _state = ShipState.Move;
            }, _data.appearanceRate);
        }

        private bool _isOnMove;
        private void Move(float inDeltaTime)
        {
            _movementTimeCounter += inDeltaTime;
            _positionRatio = _movementTimeCounter / _data.duration;
            if (_positionRatio >= 1f)
            {
                Reset();
                _isOnMove = false;
                AudioSystem.Stop(AudioLabel.SpecialMove);
                _state = ShipState.None;
                return;
            }
            
            transform.localPosition =
                Vector3.Lerp(_data.startPos, _data.targetPos, _data.moveEase.Evaluate(_positionRatio));
            
            if (_isOnMove) return;
                _isOnMove = true;
                AudioSystem.Play(AudioLabel.SpecialMove);
        }

        public void TakeDamage()
        {
            if (!IsAlive) return;
            Health--;
        }

        public int Kill()
        {
            IsAlive = false;
            _health = 0;
            AudioSystem.Stop(AudioLabel.SpecialMove);
            AudioSystem.Play(AudioLabel.SpecialDie);
            PlayKillAnimation();
            return CurrentScore;
        }

        private void PlayKillAnimation()
        {
            var animationData = _data.killAnimation;
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
                })
                .Play();
        }
    }
}