using System;
using Core;
using UnityEngine;
using Utils;

namespace Views
{
    public enum CollisionType
    {
        BoxCollider, HitDetector    
    }
    
    public class GameView3D : GameView
    {
        protected LayerMask layerMask
        {
            get => gameObject.layer;
            set => gameObject.layer = value;
        }

        public HitDetector hitDetector
        {
            get
            {
                if (_hit != null) return _hit;
                if(_collisionType != CollisionType.HitDetector)
                    GameExceptions.Exception($"Collision type mismatch !");
                _hit = AddComponent<HitDetector>();
                return _hit;
            }
        }

        private readonly CollisionType _collisionType;
        private HitDetector            _hit = null;
        
        public GameView3D(string inName, Mesh inMesh, CollisionType inCollision = CollisionType.BoxCollider, Material inMaterial = null) : base(inName)
        {
            _collisionType = inCollision;
            AssignUnityComponents();
            UpdateShape(inMesh, inMaterial);
        }

        private void AssignUnityComponents()
        {
            AddComponent<MeshRenderer>();
            AddComponent<MeshFilter>();
            
            if(_collisionType == CollisionType.BoxCollider)
                AddComponent<BoxCollider>().isTrigger = true;
        }
        
        protected void UpdateShape(Mesh inMesh, Material inMaterial)
        {
            var material = inMaterial == null
                ? Main.Settings.defaultMaterial
                : inMaterial;
            
            GetComponent<MeshFilter>().mesh = inMesh;
            GetComponent<MeshRenderer>().material = material;
        }
    }
}