using System;
using System.Linq;
using System.Text;
using Core;
using Interfaces;
using UnityEngine;
using Utils;
using Object = UnityEngine.Object;

namespace Systems.Physics_System.Components
{
    public struct GameCollider : IComponent
    {
        //- events
        public event Action<GameObject> onHitEnter; 
        public event Action<GameObject> onHitStay; 
        public event Action<GameObject> onHitExit;

        public string Id { get; private set; }
        public event IComponentEvents onDestroyed;
        public GameView View { get; private set; }
        public BoxCollider Collider { get; private set; }
        
        
        public bool Enabled
        {
            get => _enabled;
            set
            {
                if(Collider == null) return;
                Collider.enabled = _enabled = value;
            }
        }
        
        public Vector3 Center
        {
            get => _center;
            set
            {
                if(Collider == null) return;
                Collider.center = _center = value;
            }
        }
        
        public Vector3 Size
        {
            get => _size;
            set
            {
                if(Collider == null) return;
                Collider.size = _size = value;
            }
        }
        
        //- private
        private Vector3 _size;
        private Vector3 _center;
        private bool _enabled;
        private Rigidbody _rigidBody;
        private GameObject _target;
        private bool _isCollide;
        private GameObject _lastCollision;
        private Collider[] _colliderBuffer;
        
        public GameCollider(GameObject inTarget, Vector3 inSize, bool inIsKinematic, bool inIsTrigger,LayerMask inMask)
        {
            Id = $"Component[{Strings.Random(16, true)}]";
            _lastCollision = null;
            _target = inTarget;
            _isCollide = false;
            _colliderBuffer = new Collider[3];
            onHitEnter = null;
            onHitStay = null;
            onHitExit = null;
            _enabled = true;
            View = null;
            onDestroyed = null;
            _mask = inMask;
            //- attach rigidBody
            _rigidBody = _target.AddComponent<Rigidbody>();
            _rigidBody.isKinematic = inIsKinematic;
            _rigidBody.collisionDetectionMode = CollisionDetectionMode.Continuous;
            
            //- attach BoxCollider
            _size = inSize;
            _center = Vector3.zero;
            Collider = _target.AddComponent<BoxCollider>();
            Collider.size = _size;
            Collider.center = _center;
            Collider.isTrigger = inIsTrigger;
        }

        public void Destroy()
        {
           //onDestroyed?.Invoke(GetType(), this);
            Object.Destroy(_rigidBody);
            Object.Destroy(Collider);
            
            onHitEnter = null;
            onHitStay = null;
            onHitExit = null;
            _colliderBuffer = null;
            _lastCollision = null;
            _target = null;
            View = null;

            Main.GetSystem<PhysicsSystem>().DetachComponent(GetType(), this);
        }

        public void Attach(GameView inView)
        {
            View = inView;
        }

        private LayerMask _mask;
        public void FixedTick(float inFixedDeltaTime)
        {
            if (_target == null) return;
            var nbCollider = Physics.OverlapBoxNonAlloc(_target.transform.localPosition, Size,_colliderBuffer, Quaternion.identity, _mask);
            _isCollide = nbCollider > 1;
            
            if (_isCollide)
            {
                if (_lastCollision == null)
                {
                    //- Hit Enter
                    _lastCollision = _colliderBuffer[1].gameObject;
                    Debug.Log($"Hit Enter {_lastCollision.name}");
                    onHitEnter?.Invoke(_lastCollision);
                    return;
                }
                //- Hit Stay
                Debug.Log($"Hit Stay {_colliderBuffer[1].gameObject.name}");
                onHitStay?.Invoke(_colliderBuffer[1].gameObject);
            }
            else if (_lastCollision != null)
            {
                //- Hit Exit
                Debug.Log($"Hit Exit {_lastCollision.name}");
                onHitExit?.Invoke(_lastCollision);
                _lastCollision = null;
            }
            else
            {
                Debug.Log($"No Collision");
                _lastCollision = null;
            }
        }
    }
}