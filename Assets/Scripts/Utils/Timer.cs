using System;
using Systems;
using Core;

namespace Utils
{
    public class Timer
    {
        private TimingSystem _timmingSystem 
            => _tSystem ?? (_tSystem = Main.GetSystem<TimingSystem>());

        private readonly float _duration;
        private TimingSystem   _tSystem;
        private float          _timeCounter;
        private bool           _isRun;
        private Action         _callback;
        
        public Timer(float inDuration, Action inCallback)
        {
            _callback = inCallback;
            _duration = inDuration;
            _isRun = false;
            _timmingSystem.Attach(this);
        }
        
        public void Tick(float inDeltaTime)
        {
            if(!_isRun) return;
            _timeCounter += inDeltaTime;
            if (_timeCounter >= _duration)
            {
                _callback?.Invoke();
                _timeCounter = 0f;
            }
        }

        public void Start()
        {
            _isRun = true;
        }

        public void Pause()
        {
            _isRun = false;
        }

        public void Stop()
        {
            _isRun = false;
            _timeCounter = 0f;
        }

        public void Destroy()
        {
            _timmingSystem.Detach(this);
            
            // release references
            _callback = null;
            _tSystem = null;
        }

    }
}