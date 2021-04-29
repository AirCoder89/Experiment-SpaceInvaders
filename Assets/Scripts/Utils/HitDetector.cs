using System;
using AirCoder.TJ.Core;
using UnityEngine;

namespace Utils
{
    public class HitDetector : BaseMonoBehaviour
    {
        public bool Enabled
        {
            get => _isEnabled;
            set
            {
                _isEnabled = value;
                _collider.enabled = _isEnabled;
            }
        }

        public event Action<GameObject> onHitEnter;
        public event Action<GameObject> onHitStay;
        public event Action<GameObject> onHitExit;
        
        private BoxCollider _collider;
        private Rigidbody   _rigidBody;
        private LayerMask   _targetLayer;
        private bool        _isEnabled;
        
        public void Initialize(Vector3 inSize, bool isTrigger, LayerMask inLayerMask)
        {
            _targetLayer = inLayerMask;
            
            if(_rigidBody == null) 
                _rigidBody = gameObject.AddComponent<Rigidbody>();
            _rigidBody.useGravity = false;
            _rigidBody.isKinematic = true;
            
           if(_collider == null) 
               _collider = gameObject.AddComponent<BoxCollider>();
           _collider.isTrigger = isTrigger;
           _collider.size = inSize;

           Enabled = true;
        }

        public void UpdateLayerMask(LayerMask inLayerMask)
            => _targetLayer = inLayerMask;

        protected override void ReleaseReferences()
        {
            _rigidBody = null;
            _collider = null;
            onHitEnter = null;
            onHitStay = null;
            onHitExit = null;
        }
        
        private bool CompareLayer(LayerMask target, LayerMask other)
            => (target.value & 1 << other) == 1 << other;
        
        private void OnTriggerEnter(Collider other)
        {
            if(!Enabled) return;
            if(!CompareLayer(_targetLayer, other.gameObject.layer)) return;
            onHitEnter?.Invoke(other.gameObject);
        }

        private void OnTriggerStay(Collider other)
        {
            if(!Enabled) return;
            if(!CompareLayer(_targetLayer, other.gameObject.layer)) return;
            onHitStay?.Invoke(other.gameObject);
        }

        private void OnTriggerExit(Collider other)
        {
            if(!Enabled) return;
            if(!CompareLayer(_targetLayer, other.gameObject.layer)) return;
            onHitExit?.Invoke(other.gameObject);
        }

        private void OnCollisionEnter(Collision other)
        {
            if(!Enabled) return;
            if(!CompareLayer(_targetLayer, other.gameObject.layer)) return;
            onHitEnter?.Invoke(other.gameObject);
        }

        private void OnCollisionStay(Collision other)
        {
            if(!Enabled) return;
            if(!CompareLayer(_targetLayer, other.gameObject.layer)) return;
            onHitStay?.Invoke(other.gameObject);
        }

        private void OnCollisionExit(Collision other)
        {
            if(!Enabled) return;
            if(!CompareLayer(_targetLayer, other.gameObject.layer)) return;
            onHitExit?.Invoke(other.gameObject);
        }
    }
}