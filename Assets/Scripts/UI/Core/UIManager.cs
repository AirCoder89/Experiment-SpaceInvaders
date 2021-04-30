using UnityEngine;

namespace UI.Core
{
    public sealed class UIManager: MonoBehaviour
    {
        [Range(0,1)] public float pos;
        [Range(0,1)] public float sinVal;
        
        public UIState[] states;

        public void Initialize()
        {
            foreach (var state in states)
                state.Initialize(this);
        }

        private void Update()
        {
            sinVal = Mathf.Clamp(Mathf.Sin(pos * Mathf.PI), 0f, 1f);
        }
    }
}