using System;
using AirCoder.TJ.Core.Extensions;
using AirCoder.TJ.Core.Jobs;
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

        private GridSystem     _grid;
        private InvadersConfig _config;
        private Direction      _currentDirection;
        private float          _timeCounter;
        private bool           _canMove;
        private bool           _hasToReverse;
        private bool           _showSpecialShip;
        
        private GameView3D _specialShip;
        private Timer      _specialShipTimer;
        
        public InvadersSystem(SystemConfig inConfig = null) : base(inConfig)
        {
            if(inConfig != null) _config = inConfig as InvadersConfig;
            CreateEdges();
            
            //- Special Ship
            /*_showSpecialShip = false;
            _specialShip = new GameView3D("SpecialShip", _config.specialShipMesh);
            _specialShip.SetPosition(_config.specialShipStartPos);
            _specialShipTimer = new Timer(_config.specialShipAppearanceRate, ShowSpecialShip);*/
        }

        private void ShowSpecialShip()
        {
            _specialShipTimer.Stop();_specialShip.gameObject.transform
                .TweenLocalPosition(_config.specialShipTargetPos, _config.specialShipSpeed)
                .OnComplete(() =>
                {
                    _specialShip.SetPosition(_config.specialShipStartPos);
                    _specialShipTimer.Start();
                }).Play();
        }

        public override void Start()
        {
            _currentDirection = Direction.Right;
            _timeCounter = 0f;
            _canMove = true;
            //_specialShipTimer.Start();
        }

        public void Tick(float inDeltaTime)
        {
            if(!IsRun|| !_gridSystem.IsReady) return;
            if (_canMove)
            {
                _timeCounter += inDeltaTime;
                MoveInvadersLeftAndRight();
            }
            
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
        
        private void CreateEdges()
        {
            //- Left Edge
            var leftEdge = new GameObject("LeftEdge");
            leftEdge.transform.position = _config.edges.leftPos;
            var leftHitEdge = leftEdge.AddComponent<HitDetector>();
            leftHitEdge.Initialize(_config.edges.size, false);
            leftHitEdge.onHitEnter += o => _hasToReverse = true;
            
            //- Right Edge
            var rightEdge = new GameObject("RightEdge");
            rightEdge.transform.position = _config.edges.rightPos;
            var rightHitEdge = rightEdge.AddComponent<HitDetector>();
            rightHitEdge.Initialize(_config.edges.size, false);
            rightHitEdge.onHitEnter += o => _hasToReverse = true;
        }

    }
}