using Models.Animations;
using UnityEngine;

namespace Core
{
    [CreateAssetMenu(menuName = "Game/Settings")]
    public class GameSettings : ScriptableObject
    {
        public GameLoop    gameLoop;
        public States      startState;
        public Material    defaultMaterial;
        public int         targetFrameRate;
        public bool        cursorVisibility;
        public bool        vSync;
    }
}