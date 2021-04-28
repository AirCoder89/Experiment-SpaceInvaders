using Core;
using UnityEngine;

namespace Views
{
    public class GameView3D : GameView
    {
        protected MeshRenderer Renderer;
        protected MeshFilter   Filter;
        protected BoxCollider  Collider;

        public GameView3D(string inName, Mesh inMesh, Material inMaterial = null) : base(inName)
        {
            AssignUnityComponents();
            UpdateShape(inMesh, inMaterial);
        }

        public override void Destroy()
        {
            base.Destroy();
            
            Object.Destroy(Renderer);
            Renderer = null;
            
            Object.Destroy(Filter);
            Filter = null;
            
            Object.Destroy(Collider);
            Collider = null;
        }

        private void AssignUnityComponents()
        {
            Renderer = gameObject.AddComponent<MeshRenderer>();
            Filter = gameObject.AddComponent<MeshFilter>();
            Collider = gameObject.AddComponent<BoxCollider>();
            Collider.isTrigger = true;
        }
        
        protected void UpdateShape(Mesh inMesh, Material inMaterial)
        {
            var material = inMaterial == null
                ? Main.Settings.defaultMaterial
                : inMaterial;
            
            Filter.mesh = inMesh;
            Renderer.material = material;
        }
    }
}