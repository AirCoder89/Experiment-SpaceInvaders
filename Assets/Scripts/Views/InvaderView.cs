using Models.SystemConfigs;
using UnityEngine;
using Utils.Array2D;
using Vector2Int = Utils.Array2D.Vector2Int;

namespace Views
{
    public class InvaderView : Cell
    {
        private GridConfig _gridConfig;

        private MeshRenderer _renderer;
        private MeshFilter _filter;
        private BoxCollider _collider;
        
        public InvaderView(string inName, Vector2Int inLocation, Mesh inMesh, GridConfig inConfig) : base(inName, inLocation)
        {
            _gridConfig = inConfig;
            UpdatePosition();
            AssignUnityComponents(inMesh);
        }

        private void AssignUnityComponents(Mesh inMesh)
        {
            _renderer = gameObject.AddComponent<MeshRenderer>();
            _filter = gameObject.AddComponent<MeshFilter>();
            _collider = gameObject.AddComponent<BoxCollider>();

            _filter.mesh = inMesh;
            _renderer.material = _gridConfig.material;
        }
        
        public void UpdatePosition()
            => gameObject.transform.position = LocationToPosition(Location);
        
        private Vector3 LocationToPosition(Vector2Int inLocation)
        {
            var spacing = new Vector3(_gridConfig.spacing.x * inLocation.y, -_gridConfig.spacing.y * inLocation.x, 0f);
            var slotPosition = new Vector3(_gridConfig.cellSize.x * inLocation.y, -_gridConfig.cellSize.y * inLocation.x, 0f);
            return (Vector3)_gridConfig.padding + slotPosition + spacing;
        }

    }
}