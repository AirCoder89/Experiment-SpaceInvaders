using Core;
using Models.SystemConfigs;
using UnityEngine;
using Utils.Array2D;
using Views;
using Vector2Int = Utils.Array2D.Vector2Int;

namespace Systems
{
    public class GridSystem : GameSystem
    {
        private GridConfig _config;
        private Transform _gridHolder;
        private Matrix<InvaderView> _matrix;
        
        public GridSystem(SystemConfig inConfig = null) : base(inConfig)
        {
            if(inConfig != null) _config = inConfig as GridConfig;
            _gridHolder = new GameObject("Grid Holder").transform;
            _gridHolder.position = Vector3.zero;
        }

        public override void Start()
        {
            GenerateEmpty();
        }
        
        private void GenerateEmpty()
        {
            _matrix = new Matrix<InvaderView>(_config.dimension.y, _config.dimension.x);
            
            for (var y = 0; y < _config.dimension.y; y++)
            {
                for (var x = 0; x < _config.dimension.x; x++)
                {
                    _matrix[y, x] = new InvaderView($"Invader[{y},{x}]", new Vector2Int(y, x), _config.mesh, _config);
                    _matrix[y, x].SetParent(_gridHolder);
                }
            }
        }

       
    }
}