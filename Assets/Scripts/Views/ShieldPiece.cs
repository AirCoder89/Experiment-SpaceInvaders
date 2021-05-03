using Models.SystemConfigs;
using Interfaces;
using Utils;

namespace Views
{
    public class ShieldPiece : GameView3D, IDestructible
    {
        public bool                   IsAlive { get; private set; }
        public int Health
        {
            get => _health;
            private set
            {
                _health = value;
                IsAlive = _health > 0;
                if (IsAlive) return;
                    Kill();
            }
        }
        
        private readonly int  _startHealth;
        private int           _health;
        
        public ShieldPiece(string inName, ShieldConfig inConfig)  : base(inName, inConfig.pieceData.meshPiece)
        {
            _startHealth = Health = inConfig.pieceData.shieldHealth;
            layerMask = LayersList.Shields;
        }
        
        public void TakeDamage()
        {
            if(!IsAlive) return;
            Health--;
            //Todo : update shape
        }

        public int Kill()
        {
            Visibility = false;
            return 0;
        }

        public void Reset()
        {
            Health = _startHealth;
            Visibility = true;
        }
    }
}