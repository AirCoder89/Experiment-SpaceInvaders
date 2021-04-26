namespace Utils.Array2D
{
    [System.Serializable]
    public struct Vector2Int
    {
        public int x;
        public int y;

        public Vector2Int(int inX, int inY)
        {
            x = inX;
            y = inY;
        }

        public override string ToString()
            => $"[{x}][{y}]";
    }
}