using System;
using System.Collections.Generic;
using Core;
using Interfaces;
using Models.SystemConfigs;
using UnityEngine;
using Utils;
using Utils.Array2D;
using Views;
using Vector2Int = Utils.Array2D.Vector2Int;

namespace Systems
{
    public class GridSystem : GameSystem, ITick
    {
        public bool IsReady { get; private set; }
        public Matrix<InvaderView> Matrix => _matrix;
        
        private readonly GridConfig _config;
        private readonly Transform  _gridHolder;
        private Matrix<InvaderView> _matrix;
        
        public GridSystem(SystemConfig inConfig = null) : base(inConfig)
        {
            if(inConfig != null) _config = inConfig as GridConfig;
            _gridHolder = new GameObject("Grid Holder").transform;
            _gridHolder.SetParent(GameState.GameHolder);
            _gridHolder.position = Vector3.zero;
            IsReady = false;
        }

        public override void Start()
        {
            Generate();
            IsReady = false;
            if(_config.animation.openingAnimation.axis == TweenAxis.Vertical) OpeningAnimation(_matrix.Columns);
            else if(_config.animation.openingAnimation.axis == TweenAxis.Horizontal) OpeningAnimation(_matrix.Rows);
        }

        public void NotReady() => IsReady = false;
        
        public void Reset()
        {
            for (var y = 0; y < _config.establishing.dimension.y; y++)
            {
                var invadersData = _config.invaders.GetDataByInvaderIndex(y);
                for (var x = 0; x < _config.establishing.dimension.x; x++)
                {
                    invadersData.color = _config.invaders.GetRandomColor();
                    invadersData.position = new Vector2Int(y, x);
                    var invaderView = _matrix[y, x] as InvaderView;
                    invaderView?.BindData(invadersData);
                }
            }
            if(_config.animation.openingAnimation.axis == TweenAxis.Vertical) OpeningAnimation(_matrix.Columns);
            else if(_config.animation.openingAnimation.axis == TweenAxis.Horizontal) OpeningAnimation(_matrix.Rows);
        }
        
        private void Generate()
        {
            _matrix = new Matrix<InvaderView>(_config.establishing.dimension.y, _config.establishing.dimension.x);
            
            for (var y = 0; y < _config.establishing.dimension.y; y++)
            {
                var invadersData = _config.invaders.GetDataByInvaderIndex(y);
                for (var x = 0; x < _config.establishing.dimension.x; x++)
                {
                    invadersData.color = _config.invaders.GetRandomColor();
                    invadersData.position = new Vector2Int(y, x);
                    
                    _matrix[y, x] = new InvaderView($"Invader[{y},{x}]", invadersData , _config);
                    _matrix[y, x].SetParent(_gridHolder);
                }
            }
        }

        /* in this approach I want to demonstrate sequence animations using callbacks.
               It's may consume a little bit more than interpolate the scale them with Update.
               We got benefits like flexibility to control the animation (LTR / RTL) or (Vertical/Horizontal) 
               and by using tween library, we can animate using ease equations and that will smooth the animation.
               also 
            */
        private void OpeningAnimation(Line2D<Cell>[] inLines)
        {
            //- end to start
            var animationData = _config.animation.openingAnimation;
            for (var i = 0; i < inLines.Length; i++)
            {
                inLines[i].TweenCellsScale(animationData.direction, animationData.speed, animationData.ease);
                if (i == 0) inLines[i].TweenCallback = () => { IsReady = true;};
                else
                {
                    var index = i;
                    inLines[i].TweenCallback = () => {inLines[index-1].PlayScaleTween(animationData.direction); };
                }
            }
            inLines[inLines.Length-1].PlayScaleTween(animationData.direction); 
        }

        /// <summary>
        /// when we want to check if the level is completed so we start iterating matrix column by column
        /// and from each column we try to get the first (from top) alive invader. if we got one alive we break and
        /// return false. otherwise if all column return null when we call GetFirstInvader that is mean all invaders has been destroyed.
        /// </summary>
        public bool IsLevelWin()
        {
            for (var i = 0; i < _matrix.Columns.Length; i++)
            {
                var lastInvader = GetFirstInvader(i);
                if (lastInvader != null) return false;
            }
            return true;
        }
        
        /// <returns>return the first alive invader from to given column index</returns>
        public InvaderView GetFirstInvader(int inColumn)
        {
            var column = _matrix.Columns[inColumn];
            if(!(column.GetFirstAlive() is InvaderView lastAlive)) return null;
            return lastAlive;
        }
        
        /// <returns>return the last alive invader from to given column index</returns>
        public InvaderView GetLastInvader(int inColumn)
        {
            var column = _matrix.Columns[inColumn];
            if(!(column.GetLastAlive() is InvaderView lastAlive)) return null;
            return lastAlive;
        }

        public List<Cell> GetMatches(Vector2Int inLocation)
            => _matrix.GetMatches(_matrix[inLocation]);

        public void Tick(float inDeltaTime)
        {
            if(!IsRun) return;
            _matrix.Tick(inDeltaTime);
        }

        public void MoveLine(Direction inDirection, float inStepLength, float inStepDuration, Action onComplete)
        {
            AudioSystem.Play(AudioLabel.InvadersMove);
            _matrix.MoveLineTo(_config.animation.basedLine, inDirection, inStepLength, inStepDuration, onComplete);
        }

        
    }
}