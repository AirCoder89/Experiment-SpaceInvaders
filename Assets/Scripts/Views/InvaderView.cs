using Models;
using Models.SystemConfigs;
using UnityEngine;
using Utils.Array2D;
using Vector2Int = Utils.Array2D.Vector2Int;

namespace Views
{
    public class InvaderView : Cell
    {
        private GridConfig _gridConfig;
       
        public InvaderView(string inName, CellData inData, GridConfig inConfig)  : base(inName, inData.position, inData, inData.mesh, inConfig.material)
        {
            _gridConfig = inConfig;
            UpdatePosition();
             Renderer.material.color = inData.color;
             SetScale(Vector3.zero);
        }
       
        public void UpdatePosition()
            => gameObject.transform.position = LocationToPosition(Location);
        
        private Vector3 LocationToPosition(Vector2Int inLocation)
        {
            var spacing = new Vector3(_gridConfig.spacing.x * inLocation.y, -_gridConfig.spacing.y * inLocation.x, 0f);
            var position = new Vector3(_gridConfig.cellSize.x * inLocation.y, -_gridConfig.cellSize.y * inLocation.x, 0f);
            return (Vector3)_gridConfig.padding + position + spacing;
        }

        public override void Destroy()
        {
            base.Destroy();
            _gridConfig = null;
        }

        public void Select()
        {
            Renderer.material.color = Color.magenta;
        }

        public void UnSelect()
        {
            Renderer.material.color = Data.color;
        }

    }
}