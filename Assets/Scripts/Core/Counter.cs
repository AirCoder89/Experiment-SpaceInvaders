
namespace Core
{
    public class Counter
    {
        public float TimeRatio => _currentTime / _targetTime;
        
        private readonly float _targetTime;
        private float          _currentTime;
        private bool           _isCounting;

        public Counter(float time)
        {
            _targetTime = time;
        }
        
        public void Start()
        {
            _currentTime = 0f;
            _isCounting = true;
        }

        public void Stop()
        {
            _isCounting = false;
        }

        public void Tick(float inDeltaTime)
        {
            if(!_isCounting) return;
            if (_currentTime >= _targetTime)
            {
                _isCounting =false;
                return;
            }
            _currentTime += inDeltaTime;
        }
        
    }
}