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

        private Line2D<Cell>[] _columns;
        private Line2D<Cell>[] _rows;

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

        private List<Cell> _matchesCells;
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

    }
}
