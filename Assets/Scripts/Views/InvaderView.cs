using System;
using Systems;
using AirCoder.TJ.Core.Extensions;
using Core;
using Interfaces;
using Models;
using Models.Grid;
using Models.SystemConfigs;
using UnityEngine;
using Utils.Array2D;
using Vector2Int = Utils.Array2D.Vector2Int;

namespace Views
{
    public class InvaderView : Cell, IDestructible
    {
        public static event Action<GameView> OnDestroyed;
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
        
        private ShootingSystem  _shoot;
        private readonly int    _startHealth;
        private GridConfig      _gridConfig;
        private int             _health;
        
        public InvaderView(string inName, CellData inData, GridConfig inConfig)  
            : base(inName, inData.position, inData, inData.meshes[0], inConfig.establishing.material)
        {
            _startHealth = Health = 1; //one hit
            
            _gridConfig = inConfig;
            UpdatePosition();
             GetComponent<MeshRenderer>().material.color = inData.color;
             SetScale(Vector3.zero);
             
             //-adjust collider
             GetComponent<BoxCollider>().center = new Vector3(0f, 0.18f, 0f);
             GetComponent<BoxCollider>().size = new Vector3(1f, 0.81f, 1f);
        }

        public override void BindData(CellData inData)
        {
            base.BindData(inData);
            UpdatePosition();
            GetComponent<MeshRenderer>().material.color = inData.color;
        }
       
        public void UpdateMeshByIndex(int index)
        {
            UpdateMesh(Data.meshes[index]);
        }
        
        public void UpdatePosition()
            => gameObject.transform.position = LocationToPosition(Location);
        
        private Vector3 LocationToPosition(Vector2Int inLocation)
        {
            var spacing = new Vector3(_gridConfig.establishing.spacing.x * inLocation.y, -_gridConfig.establishing.spacing.y * inLocation.x, 0f);
            var position = new Vector3(_gridConfig.establishing.cellSize.x * inLocation.y, -_gridConfig.establishing.cellSize.y * inLocation.x, 0f);
            return (Vector3)_gridConfig.establishing.padding + position + spacing;
        }

        public override void Destroy()
        {
            base.Destroy();
            _gridConfig = null;
        }

        public void TakeDamage()
        {
            if(!IsAlive) return;
            Health--;
        }

        public void Kill()
        {
            IsAlive = false;
            _health = 0;
            PlayKillAnimation();
        }

        private void PlayKillAnimation()
        {
            var animationData = _gridConfig.animation.killAnimation;
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

        public void Revive()
        {
            Health = _startHealth;
            Visibility = true;
        }

        public void Select()
        {
            GetComponent<MeshRenderer>().material.color = Color.magenta;
        }

        public void UnSelect()
        {
            GetComponent<MeshRenderer>().material.color = Data.color;
        }

        public void Shoot(LayerMask inLayerMask)
        {
            _shootingSystem.Shoot(Position, Vector3.down, inLayerMask);
        }
    }
}