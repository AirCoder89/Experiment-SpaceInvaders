using Core;
using UnityEngine;

namespace Models.SystemConfigs
{
    public enum InputType
    {
        Editor, Touch
    }

    [CreateAssetMenu(menuName = "Game/System Config/Input System Config")]
    public class InputConfig : SystemConfig
    {
        public bool autoDetect = true;
        public InputType inputType;
    }
}