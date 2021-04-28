using System;
using AirCoder.TJ.Core;
using UnityEngine;

namespace Utils
{
    public class HitDetector : BaseMonoBehaviour
    {
        public event Action<GameObject> onHitEnter;
        public event Action<GameObject> onHitStay;
        public event Action<GameObject> onHitExit;
        
        private BoxCollider _collider;
        private Rigidbody   _rigidBody;
        
        public void Initialize(Vector3 inSize, bool isTrigger)
        {
            if(_rigidBody == null) 
                _rigidBody = gameObject.AddComponent<Rigidbody>();
            _rigidBody.useGravity = false;
            _rigidBody.isKinematic = true;
            
           if(_collider == null) 
               _collider = gameObject.AddComponent<BoxCollider>();
           _collider.isTrigger = isTrigger;
           _collider.size = inSize;
           
        }

        private void OnTriggerEnter(Collider other) => onHitEnter?.Invoke(other.gameObject);
        private void OnTriggerStay(Collider other) => onHitStay?.Invoke(other.gameObject);
        private void OnTriggerExit(Collider other) => onHitExit?.Invoke(other.gameObject);

        private void OnCollisionEnter(Collision other) => onHitEnter?.Invoke(other.gameObject);
        private void OnCollisionStay(Collision other) => onHitStay?.Invoke(other.gameObject);
        private void OnCollisionExit(Collision other) => onHitExit?.Invoke(other.gameObject);


        protected override void ReleaseReferences()
        {
            _rigidBody = null;
            _collider = null;
            onHitEnter = null;
            onHitStay = null;
            onHitExit = null;
        }
    }
}