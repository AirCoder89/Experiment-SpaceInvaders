using System;

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
    }
}
