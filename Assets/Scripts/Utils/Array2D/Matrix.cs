using System;
using System.Collections.Generic;
using System.Linq;
using Views;

namespace Utils.Array2D
{
    [Serializable]
    public class Matrix<T> where T : Cell
    {
        public Vector2Int Dimension { get; private set; }
        public Line2D<Cell>[] Columns => _columns;
        public Line2D<Cell>[] Rows => _rows;
        
        public Cell this[int x, int y]
        {
            get => _columns[y][x];
            set
            {
                _columns[y][x] = value;
                _rows[x][y] = value;
            }
        }

        private Line2D<Cell>[]  _columns;
        private Line2D<Cell>[]  _rows;
        private List<Cell>      _matchesCells;
        
        public Matrix(int width, int height)
        {
            _columns = new Line2D<Cell>[height];
            _rows = new Line2D<Cell>[width];
            Dimension = new Vector2Int(width, height);
            
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    _columns[y] = new Line2D<Cell>(width);
                    _rows[x] = new Line2D<Cell>(height);
                }
            }
        }

        public List<Cell> GetMatches(Cell inCell)
        {
            if (_matchesCells == null) _matchesCells = new List<Cell>();
            else this._matchesCells.Clear();
            
            ResetVisitedCells();

            var initialResult = inCell.GetMatchesNeighbors(this);
            _matchesCells.AddRange(initialResult);
       
            while (true) {
                var allVisited = true;
                for (var i = _matchesCells.Count - 1; i >= 0 ; i--) {
                    var b = _matchesCells [i];
                    if (!b.IsVisited)
                    {
                        AddMatches (b.GetMatchesNeighbors(this));
                        allVisited = false;
                    }
                }
                if (allVisited)  return _matchesCells;
            }
        }
        
        private void AddMatches (IEnumerable<Cell> matches) 
        {
            foreach (var cell in matches) 
            {
                if (!_matchesCells.Contains(cell))
                    _matchesCells.Add(cell);
            }
            _matchesCells.Distinct();
        }

        private void ResetVisitedCells()
        {
            for (var y = 0; y < Dimension.y; y++)
            {
                for (var x = 0; x < Dimension.x; x++)
                    _columns[y][x].IsVisited = false;
            }
        }

        public void Tick(float inDeltaTime)
        {
            for (var y = 0; y < Dimension.y; y++)
            {
                for (var x = 0; x < Dimension.x; x++)
                {
                    _columns[y].Tick(inDeltaTime);
                    _rows[x].Tick(inDeltaTime);
                }
            }
        }

        //-------- Animations Handlers
        private MatrixLine _targetLine;
        private Direction _direction;
        private Action _moveCallback;
        
        public void MoveLineTo(MatrixLine inLine, Direction inDirection, float inStepLength, float inStepDuration, Action inCallback = null)
        {
            _targetLine = inLine;
            _direction = inDirection;
            _moveCallback = inCallback;
            MoveTargetLines(GetStartIndex(), inStepLength, inStepDuration);
        }

        private Line2D<Cell>[] GetTargetLines()
            => _targetLine == MatrixLine.Row ? this.Rows : this.Columns;
        
        private void MoveTargetLines(int index, float inStepLength, float inStepDuration)
        {
            if (IsLineCompleted(index))
            {
                //completed
                _moveCallback?.Invoke();
                return;
            }
            GetTargetLines()[index].MoveTo(_direction, inStepLength, inStepDuration, () =>
            {
                //next line
                var next = NextLine(index);
                MoveTargetLines(next, inStepLength, inStepDuration);
            });
        }

        private int GetStartIndex()
        {
            if (_direction == Direction.Up) return 0;
            return GetTargetLines().Length - 1;
        }
        
        private int NextLine(int index)
        {
            if (_direction == Direction.Up) index++;
            else index--;
            return index;
        }
        
        private bool IsLineCompleted(int index)
        {
            if (_direction == Direction.Up) return index >= GetTargetLines().Length;
            else return index < 0;
        }
    }
}
