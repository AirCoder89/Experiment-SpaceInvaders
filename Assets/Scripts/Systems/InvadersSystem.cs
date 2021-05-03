using System;
using Core;
using Interfaces;
using Models.SystemConfigs;
using Utils;
using Utils.Array2D;
using Views;
using Random = UnityEngine.Random;

namespace Systems
{
    public class InvadersSystem : GameSystem, ITick
    {
        public static event Action<int> OnCollectScore; 
        private GridSystem _gridSystem 
            => _grid ?? (_grid = Main.GetSystem<GridSystem>());

        private readonly InvadersConfig _config;
        private readonly SpecialShip    _specialShip;
        private readonly Timer          _shootingTimer;
        private GridSystem              _grid;
        private Direction               _currentDirection;
        private float                   _timeCounter;
        private bool                    _canMove;
        private bool                    _hasToReverse;
        
        public InvadersSystem(SystemConfig inConfig = null) : base(inConfig)
        {
            if(inConfig != null) _config = inConfig as InvadersConfig;
            if(inConfig == null) GameExceptions.NullReference($"InvadersConfig must be not null!");
            
            _shootingTimer = new Timer(_config.behaviours.shootingRate, Shoot);
            
            InvaderView.OnDestroyed += OnDestroyInvader;
            LevelSystem.OnHitVerticalEdges += () => _hasToReverse = true;
            
            //- create special ship
            _specialShip = new SpecialShip("SpecialShip", _config.specialShip);
            _specialShip.SetParent(GameState.GameHolder);
        }
        
        public Action Reset()
        {
            _shootingTimer.Stop();
            _hasToReverse = false;
            _canMove = false;
            _currentDirection = Direction.Right;
            _timeCounter = 0f;
            _specialShip.Reset();

            return OnResetComplete;
        }

        private void OnResetComplete()
        {
            _canMove = true;
            _shootingTimer.Start();
        }
        
        public override void Start()
        {
            _currentDirection = Direction.Right;
            _timeCounter = 0f;
            _specialShip.Reset();
            _canMove = true;
            _shootingTimer.Start();
        }

        private void OnDestroyInvader(InvaderView inTarget)
        {
            var hitScore = inTarget.Kill();
            AudioSystem.Play(AudioLabel.HitInvaders);
            hitScore += DestroyMatches(inTarget);
            OnCollectScore?.Invoke(hitScore);
        }

        private int DestroyMatches(Cell inTarget)
        {
            if(inTarget == null) return 0;
                var matchesScore = 0;
                var matches = _gridSystem.GetMatches(inTarget.Location);
                foreach (var cell in matches)
                {
                    if (cell is IDestructible destructible)
                        matchesScore += destructible.Kill();
                }
                return matchesScore;
        }

        private void Shoot()
        {
            if(!_gridSystem.IsReady) return;
            var randomColumn = Random.Range(0, _gridSystem.Matrix.Columns.Length);
            var lastInvader = _gridSystem.GetLastInvader(randomColumn);
            lastInvader?.Shoot(_config.behaviours.targetLayer);
        }

        public void Tick(float inDeltaTime)
        {
            if(!IsRun|| !_gridSystem.IsReady) return;
            if (_canMove)
            {
                _timeCounter += inDeltaTime;
                MoveInvadersLeftAndRight();
            }
            _specialShip.Tick(inDeltaTime);
        }

        private void MoveInvadersLeftAndRight()
        {
            var step = _timeCounter / _config.movement.stepDelay;
            if (!(step >= 1f)) return;
                //move
                _canMove = false;
                _timeCounter = 0f;
                MoveToDirection();
        }

        private void MoveToDirection()
        {
            _gridSystem.MoveLine(_currentDirection, _config.movement.stepLength, _config.movement.stepDuration, () =>
            {
                if (_hasToReverse)
                {
                    //1- Move Down
                    _hasToReverse = false;
                    _canMove = false;
                    _currentDirection = _currentDirection == Direction.Right 
                        ? Direction.Left 
                        : Direction.Right;
                    _gridSystem.MoveLine(Direction.Down, _config.movement.stepLength, _config.movement.moveDownPacing, () =>
                    {
                        //2- Continue moving Left/Right
                        _timeCounter = 0f;
                        _canMove = true;
                    });
                }
                else
                {
                    _timeCounter = 0f;
                    _canMove = true;
                }
            });
        }
    }
}