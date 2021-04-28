using System;
using AirCoder.TJ.Core;
using AirCoder.TJ.Core.Extensions;
using Interfaces;
using UnityEngine;

namespace Utils.Array2D
{
    public class Line2D<T> where T : Cell
    {
        public Action TweenCallback { get; set; }
        public bool Visibility
        {
            get => _visibility;
            set
            {
                _visibility = value;
                foreach (var cell in _values)
                    cell.Visibility = _visibility;
            }
        }
        
        private readonly T[] _values;
        private bool         _visibility;
        private int          _index;
        private float        _counter;
        private float        _stepLength;
        private float        _stepDuration;
        private bool         _canMove;
        private Vector3      _targetPos;
        private Direction    _direction;
        private Action       _moveCallback;
        
        public T this[int i]
        {
            get => _values[i];
            set => _values[i] = value;
        }
        
        public Line2D(int inSize)
        {
            _values = new T[inSize];
        }

        public T GetLastAlive()
        {
            for (var i = _values.Length -1; i >= 0 ; i--)
            {
                if (_values[i] is IDestructible destructible && destructible.IsAlive)
                    return _values[i];
            }
            return null;
        }
        
        #region Sequence Animation Using Tick (Update)
        public void MoveTo(Direction inDirection, float inStepLength, float inStepDuration, Action inCallback = null)
        {
            if (_values == null || _values.Length == 0) return;
            _canMove = true;
            _counter = 0f;
            _moveCallback = inCallback;
            _direction = inDirection;
            _stepLength = inStepLength;
            _stepDuration = inStepDuration;
            AssignStartIndex();
            AssignTargetPosition(_values[_index]);
        }

        private void AssignTargetPosition(Cell inCell)
        {
            switch (_direction)
            {
                case Direction.Right:
                    _targetPos = new Vector3(inCell.Position.x + _stepLength, inCell.Position.y, 0f);
                    break;
                case Direction.Left:
                    _targetPos = new Vector3(inCell.Position.x - _stepLength, inCell.Position.y, 0f);
                    break;
                case Direction.Up:
                    _targetPos = new Vector3(inCell.Position.x, inCell.Position.y  + _stepLength, 0f);
                    break;
                case Direction.Down:
                    _targetPos = new Vector3(inCell.Position.x, inCell.Position.y  - _stepLength, 0f);
                    break;
            }
        }

        private Vector3 InterpolatePosition(Vector3 startPos, Vector3 targetPos, float inTime)
        {
            return startPos + (targetPos - startPos) * inTime;
        }
        
        public void Tick(float inDeltaTime)
        {
            if(!_canMove) return;
            if (IsCurrentCellCompleted())
            {
                //completed
                _moveCallback?.Invoke();
                _canMove = false;
                return;
            }
            //create cycle
            _counter += inDeltaTime;
            var step = _counter / _stepDuration;
            
            if (step >= 1f) NextCell();
            else  _values[_index].SetPosition(InterpolatePosition(_values[_index].Position, _targetPos, step));
        }

        private void AssignStartIndex()
        {
            if (_direction == Direction.Right) _index = _values.Length - 1;
            else  _index = 0;
        }
        
        private void NextCell()
        {
            _counter = 0f;
            if (_direction == Direction.Right)
            {
                _index--;
                if (_index >= 0) 
                    AssignTargetPosition(_values[_index]);
            }
            else 
            {
                _index++;
                if (_index < _values.Length) 
                    AssignTargetPosition(_values[_index]);
            }
        }
        
        private bool IsCurrentCellCompleted()
        {
            if (_direction == Direction.Right) return _index < 0;
            return _index >= _values.Length;
        }
        
        #endregion
        #region Sequence Animation Using Tween & Callbacks
        
        public void TweenCellsScale(TweenDirection inWay, float inDuration, EaseType inEase)
        {
            if (_values == null || _values.Length < 1) return;
            if (inWay == TweenDirection.RTL) AssignRTLTween(inDuration, inEase);
            else if (inWay == TweenDirection.LTR) AssignLTRTween(inDuration, inEase);
        }

        private void AssignRTLTween(float inDuration, EaseType inEase)
        {
            for (var i = 0; i < _values.Length; i++)
            {
                _values[i].TweenJob = _values[i].gameObject.transform.TweenScale(Vector3.one, inDuration).SetEase(inEase);
                if (i == 0) _values[i].TweenJob.OnComplete(() => TweenCallback?.Invoke());
                else
                {
                    var index = i;
                    _values[i].TweenJob.OnComplete((() => _values[index - 1].TweenJob.Play()));
                }
            }
        }

        private void AssignLTRTween(float inDuration, EaseType inEase)
        {
            for (var i = _values.Length - 1; i >= 0; i--)
            {
                _values[i].TweenJob = _values[i].gameObject.transform.TweenScale(Vector3.one, inDuration).SetEase(inEase);
                if (i == _values.Length - 1) _values[i].TweenJob.OnComplete(() => TweenCallback?.Invoke());
                else
                {
                    var index = i;
                    _values[i].TweenJob.OnComplete((() => _values[index + 1].TweenJob.Play()));
                }
            }
        }

        public void PlayScaleTween(TweenDirection inWay)
        {
            if (_values == null || _values.Length < 1) return;
                if(inWay == TweenDirection.RTL) _values[_values.Length-1].TweenJob.Play();
                else if(inWay == TweenDirection.LTR) _values[0].TweenJob.Play();
        }
        #endregion
      
    }
}