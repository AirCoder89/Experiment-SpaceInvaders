namespace Utils.Array2D
{
    public class Line2D<T> where T : Cell
    {
        private readonly T[] _values;
        
        private bool _visibility;
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
        
        public T this[int i]
        {
            get => _values[i];
            set => _values[i] = value;
        }

        public Line2D(int inSize)
        {
            _values = new T[inSize];
        }
    }
}