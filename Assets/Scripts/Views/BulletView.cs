using System;
using Core;
using Interfaces;
using Models;
using UnityEngine;
using Utils;

namespace Views
{
    public class BulletView : GameView, IPoolObject
    {
        public event Action<Type, GameView> onDspawn;
        
        private Vector3        _direction;
        private BulletData     _data;
        private bool           _canMove;
        
        public BulletView(string inName, BulletData inData, Transform inHolder) : base(inName)
        {
            _data = inData;
            SetParent(inHolder);
            SetPosition(Vector3.zero);
            SetScale(inData.scale);
            AddEssentialComponents();
            Visibility = false;
            _canMove = false;
        }

        private void AddEssentialComponents()
        {
            AddComponent<SpriteRenderer>().sprite = _data.sprite;
            AddComponent<HitDetector>().Initialize(_data.colliderSize, false, 0);
            GetComponent<HitDetector>().onHitEnter += OnHit;
            GetComponent<HitDetector>().Enabled = false;
        }

        private void OnHit(GameObject inOther)
        {
            Debug.Log($"Bullet Hit {inOther.name}");
            Despawn();
            var view = ObjectMap.GetView(inOther.GetInstanceID());
            if(view == null) return;
            if(view is IDestructible destructible) destructible.TakeDamage();
        }

        public void Launch(Vector3 inPosition, Vector3 inDirection, LayerMask inLayerMask)
        {
            GetComponent<HitDetector>().UpdateLayerMask(inLayerMask);
            GetComponent<HitDetector>().Enabled = true;
            Visibility = true;
            _direction = inDirection;
            transform.TransformDirection(_direction);
            SetPosition(inPosition);
            _canMove = true;
        }

        public void Tick(float inDeltaTime)
        {
            if(!_canMove) return;
            transform.Translate(_direction * (_data.speed * inDeltaTime));
        }
        
        public void Despawn()
        {
            _canMove = false;
            GetComponent<HitDetector>().Enabled = false;
            Visibility = false;
            onDspawn?.Invoke(GetType(), this);
            onDspawn = null;
        }
    }
}