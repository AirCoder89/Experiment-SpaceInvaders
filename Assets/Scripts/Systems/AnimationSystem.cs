using Core;
using Models.SystemConfigs;
using Utils;
using Views;

namespace Systems
{
    public class AnimationSystem : GameSystem
    {
        public GridSystem _gridSystem
        {
            get
            {
                if (_grid == null) _grid = Main.GetSystem<GridSystem>();
                return _grid;
            }
        }

        private readonly AnimationConfig  _config;
        private readonly Timer            _frameRateTimer;
        private GridSystem                _grid;
        private int                       _index;
        
        public AnimationSystem(SystemConfig inConfig) : base(inConfig)
        {
            if(inConfig != null) _config = inConfig as AnimationConfig;
            _index = 0;
            _frameRateTimer = new Timer(_config.duration, NextFrame);
        }

        public override void Start() => StartAnimation();
        
        private void NextFrame()
        {
            _index++;
            if (_index >= _config.meshesCount)  _index = 0;
            foreach (var row in _gridSystem.Matrix.Rows)
            {
                for (var i = 0; i < row.Count; i++)
                {
                    var invader = row[i] as InvaderView;
                    invader?.UpdateMeshByIndex(_index);
                }
            }
        }

        public void PauseAnimation() => _frameRateTimer.Pause();
        public void StartAnimation() => _frameRateTimer.Start();
        public void StopAnimation() => _frameRateTimer.Stop();

    }
}