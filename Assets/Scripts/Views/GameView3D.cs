using Core;
using UnityEngine;

namespace Views
{
    public class GameView3D : GameView
    {
        public GameView3D(string inName, Mesh inMesh, LayerMask inLayer, Material inMaterial = null) : base(inName)
        {
            AssignUnityComponents();
            UpdateShape(inMesh, inMaterial);
            gameObject.layer = inLayer;
        }

        private void AssignUnityComponents()
        {
            AddComponent<MeshRenderer>();
            AddComponent<MeshFilter>();
            AddComponent<BoxCollider>().isTrigger = true;
        }
        
        protected void UpdateShape(Mesh inMesh, Material inMaterial)
        {
            var material = inMaterial == null
                ? Main.Settings.defaultMaterial
                : inMaterial;
            
            GetComponent<MeshFilter>().mesh = inMesh;
            GetComponent<MeshRenderer>().material = material;
        }
    }
}