using Database;
using UnityEngine;

namespace Core
{
    [CreateAssetMenu(menuName = "Game/Settings")]
    public class GameSettings : ScriptableObject
    {
        public float       levelTime;
        public float       timeBonus;
        public string      currentCountry;
        public GameLoop    gameLoop;
        public States      startState;
        public Material    defaultMaterial;
        public int         targetFrameRate;
        public bool        cursorVisibility;
        public bool        vSync;
        public DbConfig    dbConfig;
    }
}