using Core;
using Interfaces;
using Models.SystemConfigs;
using UnityEngine;
using Utils;
using Utils.Array2D;
using Views;

namespace Systems
{
    public class InvadersSystem : GameSystem, ITick
    {
        private GridSystem _gridSystem 
            => _grid ?? (_grid = Main.GetSystem<GridSystem>());

        private readonly Timer _shootingTimer;
        private GridSystem     _grid;
        private InvadersConfig _config;
        private Direction      _currentDirection;
        private float          _timeCounter;
        private bool           _canMove;
        private bool           _hasToReverse;
        private bool           _moveSpecialShip;
        private float          _specialShipCounter;
        private GameView3D     _specialShip;
        
        public InvadersSystem(SystemConfig inConfig = null) : base(inConfig)
        {
            if(inConfig != null) _config = inConfig as InvadersConfig;
            _shootingTimer = new Timer(_config.behaviours.shootingRate, Shoot);
            InvaderView.OnDestroyed += view =>
            {
                AudioSystem.Play(AudioLabel.HitInvaders);
                DestroyInvaderMatches(view);
            };
            
            LevelSystem.OnHitVerticalEdges += () => _hasToReverse = true;
            CreateSpecialShip();
        }
        
        public override void Start()
        {
            _currentDirection = Direction.Right;
            _timeCounter = 0f;
            _canMove = true;
            _shootingTimer.Start();
        }

        private void CreateSpecialShip()
        {
            _moveSpecialShip = false;
            _specialShipCounter = 0f;
            _specialShip = new GameView3D("SpecialShip", _config.specialShip.mesh, LayersList.Special);
            _specialShip.SetPosition(_config.specialShip.startPos);
        }
        
        private void DestroyInvaderMatches(GameView inTarget)
        {
            if(!(inTarget is InvaderView invader)) return;
                var matches = _gridSystem.GetMatches(invader.Location);
                foreach (var cell in matches)
                {
                    if (cell is IDestructible destructible)
                        destructible.Kill();
                }
        }

        private void Shoot()
        {
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

            SpecialShipHandler(inDeltaTime);
        }

        private void SpecialShipHandler(float inDeltaTime)
        {
            _specialShipCounter += inDeltaTime;
            
            if (!_moveSpecialShip)
            { 
                if (_specialShipCounter > _config.specialShip.appearanceRate)
                {
                    //Appearance time!
                    _specialShip.SetPosition(_config.specialShip.startPos);
                    _specialShipCounter = 0f;
                    _moveSpecialShip = true;
                }
                return;
            }

            var step = _specialShipCounter / _config.specialShip.speed;
            if (step > 1f)
            {
                //interpolation complete
                _specialShipCounter = 0f;
                _moveSpecialShip = false;
            }
            else _specialShip.SetPosition(InterpolatePosition(_specialShip.Position, _config.specialShip.targetPos, step));
        }

        private Vector3 InterpolatePosition(Vector3 inPosA, Vector3 inPosB, float inTime)
            => inPosA + (inPosB - inPosA) * inTime;
        
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