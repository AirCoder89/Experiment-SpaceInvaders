using System.Collections.Generic;
using System.ComponentModel;
using Core;
using UnityEngine;

namespace Models.SystemConfigs
{
    public enum InputBehaviour
    {
        None = 0,
        Shoot = 1,
        Move = 2
    }

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