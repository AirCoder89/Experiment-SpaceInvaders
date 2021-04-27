using UnityEngine;

namespace Core
{
    [CreateAssetMenu(menuName = "Game/Settings")]
    public class GameSettings : ScriptableObject
    {
        public Material defaultMaterial;
    }
}