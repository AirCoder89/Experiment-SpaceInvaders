using Systems.Physics_System.Components;
using Models.SystemConfigs;
using UnityEngine;

namespace Views
{
    public class ShieldPiece : GameView3D
    {
        public int ElapsedHits { get; private set; }
        private GameCollider _collider;
        
        public ShieldPiece(string inName, ShieldConfig inConfig, Mesh inMesh, Material inMaterial = null) : base(inName, inMesh, inMaterial)
        {
            _collider = AddComponent(new GameCollider(gameObject, inConfig.pieceScale, true, false, inConfig.targetLayer));
            _collider.onHitEnter += OnHitEnter;
            _collider.onHitStay += OnHitStay;
            _collider.onHitExit += OnHitExit;
            gameObject.layer = inConfig.layerMask;
        }

        public override void Destroy()
        {
            base.Destroy();
        }

        private void OnHitExit(GameObject obj)
        {
            Debug.Log($"HitExit {obj.name}");
        }

        private void OnHitStay(GameObject obj)
        {
            Debug.Log($"HitStay {obj.name}");
        }

        private void OnHitEnter(GameObject obj)
        {
            Debug.Log($"HitEnter {obj.name}");
        }

    }
}