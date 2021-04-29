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
        public static event Action OnShoot;
        
        private InputConfig _config;
        
        public InputsSystem(SystemConfig inConfig) : base(inConfig)
        {
            if(inConfig != null) _config = inConfig as InputConfig;
            _config.Initialize();
        }
        
        public override void Start()
        {
            
        }

        public float GetAxis(InputBehaviour inBehaviour)
        {
            if (!IsRun || !_config) return 0f;
            return Input.GetAxisRaw(_config.GetBehaviourAxis(inBehaviour));
        }

        public void Tick(float inDeltaTime)
        {
            if(!IsRun) return;
            GetTouchInput();
        }

        private int _fingerId;

        private void GetTouchInput()
        {
            for (var i = 0; i < Input.touchCount; i++)
            {
                /*var t = Input.GetTouch(i);
                switch (t.phase)
                {
                    case TouchPhase.Began:

                        if (_fingerId == -1)
                        {
                            onFingerDown?.Invoke();
                            _fingerId = t.fingerId;
                        }

                        break;
                    case TouchPhase.Ended:
                    case TouchPhase.Canceled:

                        if (t.fingerId == _fingerId)
                        {
                            onFingerUp?.Invoke();
                            _fingerId = -1;
                        }

                        break;
                    case TouchPhase.Moved:

                        if (t.fingerId == _fingerId)
                        {
                            onMove?.Invoke();
                            _lookInput = t.deltaPosition * _settings.cameraSensitivity * Time.deltaTime;
                        }

                        break;
                    case TouchPhase.Stationary:
                        if (t.fingerId == _fingerId) _lookInput = Vector2.zero;
                        break;
                }*/
            }
        }
    }
}