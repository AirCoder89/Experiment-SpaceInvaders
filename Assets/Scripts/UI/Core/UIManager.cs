using UnityEngine;

namespace UI.Core
{
    public sealed class UIManager: MonoBehaviour
    {
        public float fadeTransitionDuration;
        public UIState[] states;

        public void Initialize()
        {
            foreach (var state in states)
                state.Initialize(this);
        }

    }
}