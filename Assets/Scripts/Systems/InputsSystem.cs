using System;
using Core;
using Interfaces;
using Models.SystemConfigs;
using UnityEngine;

namespace Systems
{
    public class InputsSystem : GameSystem,ITick
    {
        public static event Action<float> OnMove;
        public static event Action        OnShoot;
        
        private readonly InputConfig _config;
        private InputType _type;
            
        public InputsSystem(SystemConfig inConfig) : base(inConfig)
        {
            if(inConfig != null) _config = inConfig as InputConfig;
        }
        
        public override void Start()
        {
            if (_config.autoDetect)
            {
                #if UNITY_ANDROID
                    _type = InputType.Touch;
                #else
                    _type = InputType.Editor;
                #endif
            }
            else _type = _config.inputType;
        }

        public void Tick(float inDeltaTime)
        {
            if(!IsRun) return;
            if(_type == InputType.Touch)  EvaluateTouchControl();
            else if(_type == InputType.Editor)  EvaluateEditorControl();
        }

        private void EvaluateEditorControl()
        {
            var horizontal = Input.GetAxis("Horizontal");
            OnMove?.Invoke(horizontal);
            if(Input.GetKeyDown(KeyCode.UpArrow)) OnShoot?.Invoke();
        }
        
        private void EvaluateTouchControl()
        {
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                OnMove?.Invoke(Input.touches[0].deltaPosition.x);
            }
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                OnShoot?.Invoke();
            }
        }
    }
}