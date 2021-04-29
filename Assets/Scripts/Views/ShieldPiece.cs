
using System;
using Core;
using Interfaces;
using Models.SystemConfigs;
using UnityEngine;
using Utils;

namespace Views
{
    public class ShieldPiece : GameView3D, IDestructible
    {
        public event Action<GameView> onDestroyed;
        public bool                   IsAlive { get; private set; }
        public int Health
        {
            get => _health;
            private set
            {
                _health = value;
                IsAlive = _health > 0;
                if (IsAlive) return;
                //kill
                onDestroyed?.Invoke(this);
                Visibility = false;
                //Todo: add sfx
            }
        }
        
        private readonly int  _startHealth;
        private int           _health;
        
        public ShieldPiece(string inName, ShieldConfig inConfig)  : base(inName, inConfig.pieceData.meshPiece, LayersList.Shields)
        {
            _startHealth = Health = inConfig.pieceData.shieldHealth;
        }
        
        public void TakeDamage()
        {
            if(!IsAlive) return;
            Health--;
        }

        public void Revive()
        {
            Health = _startHealth;
            Visibility = true;
        }
    }

}