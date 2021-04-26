using System;
using Core;

namespace Utils.Array2D
{
    [Serializable]
    public class Cell : GameView
    {
        public Vector2Int Location { get; private set; }
        
        public Cell(string inName, Vector2Int inPosition) : base(inName)
        {
            Location = inPosition;
        }

        public Cell GetNeighbor<T>(Direction inDirection, Matrix<T> inMatrix) where T : Cell
        {
            switch (inDirection)
            {
                case Direction.Left when Location.x > 0 : return inMatrix[Location.x - 1, Location.y];
                case Direction.Right when Location.x < inMatrix.Dimension.x-1 : return inMatrix[Location.x+1, Location.y];
                case Direction.Top when Location.y > 0 : return inMatrix[Location.x, Location.y - 1];
                case Direction.Bottom when Location.y < inMatrix.Dimension.y-1 : return inMatrix[Location.x, Location.y + 1];
                default: return null;
            }
        }

        public Cell[] GetMatches<T>(Matrix<T> inMatrix) where T : Cell
        {
            return null;
        }
    }
}