using System.Collections.Generic;
using System.Linq;
using Core;
using Interfaces;
using UnityEngine;
using Utils;

namespace Systems
{
    public class TimingSystem : GameSystem, ITick
    {
        private HashSet<Timer> _timers;

        public TimingSystem() : base(null)
        {
        }
        
        public override void Start()
        {
        }

        public void Tick(float inDeltaTime)
        {
            if(!IsRun) return;
            foreach (var timer in _timers.ToList())
                timer.Tick(inDeltaTime);
        }

        public void Detach(Timer inTimer)
        {
            if(!_timers.Contains(inTimer)) return;
            _timers.Remove(inTimer);
        }

        public void Attach(Timer inTimer)
        {
            if(_timers == null) _timers = new HashSet<Timer>();
            _timers.Add(inTimer);
        }
    }
}