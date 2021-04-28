using System;
using System.Collections.Generic;
using AirCoder.TJ.Core.Extensions;
using AirCoder.TJ.Core.Jobs;
using Core;
using Models;
using UnityEngine;
using Views;

namespace Utils.Array2D
{
    [Serializable]
    public class Cell : GameView3D
    {
        public bool IsVisited { get; set; }
        protected Vector2Int Location { get; private set; }
        protected CellData Data { get; private set; }
        public ITweenJob TweenJob { get; set; }
        
        public Cell(string inName, Vector2Int inPosition, CellData inData, Mesh inMesh, Material inMaterial = null) : base(inName, inMesh, inMaterial)
        {
            Data = inData;
            Location = inPosition;
        }

        public virtual void BindData(CellData inData)
        {
            Data = inData;
            UpdateShape(Data.mesh, Renderer.material);
        }
       
        public Cell GetNeighbor<T>(Direction inDirection, Matrix<T> inMatrix) where T : Cell
        {
            switch (inDirection)
            {
                case Direction.Left when Location.x > 0 : return inMatrix[Location.x - 1, Location.y];
                case Direction.Right when Location.x < inMatrix.Dimension.x-1 : return inMatrix[Location.x+1, Location.y];
                case Direction.Up when Location.y > 0 : return inMatrix[Location.x, Location.y - 1];
                case Direction.Down when Location.y < inMatrix.Dimension.y-1 : return inMatrix[Location.x, Location.y + 1];
                default: return null;
            }
        }
        
        public List<Cell> GetAllNeighbors<T>(Matrix<T> inMatrix) where T : Cell
        {
            var result = new List<Cell>();
            if(GetNeighbor(Direction.Up, inMatrix) != null) result.Add(GetNeighbor(Direction.Up, inMatrix));
            if(GetNeighbor(Direction.Down, inMatrix) != null) result.Add(GetNeighbor(Direction.Down, inMatrix));
            if(GetNeighbor(Direction.Left, inMatrix) != null) result.Add(GetNeighbor(Direction.Left, inMatrix));
            if(GetNeighbor(Direction.Right, inMatrix) != null) result.Add(GetNeighbor(Direction.Right, inMatrix));
            return result;
        }
        
        public List<Cell> GetMatchesNeighbors<T>(Matrix<T> inMatrix) where T : Cell
        {
            var result = new List<Cell>() { this };
            var neighbors = GetAllNeighbors(inMatrix);
            IsVisited = true;
            neighbors.ForEach(neighbor =>
            {
                if(Data.color == neighbor.Data.color)
                    result.Add(neighbor);
            });
            return result;
        }
    }
}