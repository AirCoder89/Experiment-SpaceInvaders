using Core;
using UnityEngine;
using Vector2Int = Utils.Array2D.Vector2Int;

namespace Models.SystemConfigs
{
    [CreateAssetMenu(menuName = "Game/System Config/Grid System Config")]
    public class GridConfig : SystemConfig
    {
        public Vector2Int dimension;
        public Vector2 spacing;
        public Vector2 padding;
        public Vector2 cellSize;
        public Mesh mesh;
        public Material material;
    }
}