using System;
using System.Collections.Generic;
using AirCoder.TJ.Core.Ease;
using AirCoder.TJ.Core.Jobs;

namespace AirCoder.TJ.Core.Sequences
{
    public delegate void SequenceTrackingDataEvent(SequenceTrackingData sequenceData);
    
    [System.Serializable]
    public struct SequenceTrackingData
    {
        public float normalizedTime;
        public float timeRemaining;
        public ITweenJob currentJob;
        public List<ITweenJob> allJobs;
    }
    
    public interface ITweenSequence
    {
        float normalizedTime { get; set; }
        bool isPlaying { get; set; }
        void Tick(float deltaTime);
        void Play();
        void Pause();
        void Kill();
        void Initialize(SequenceType sequenceType);
        List<ITweenJob> GetJobs();
        ITweenSequence Append(ITweenJob nextJob);
        ITweenSequence Rewind();
        //chained events subscribe
        ITweenSequence SetLoop(uint nbLoop);
        ITweenSequence SetDuration(float duration);
        ITweenSequence SetEase(EaseType easeType);
        ITweenSequence OnKill(Action callback);
        ITweenSequence OnPlay(Action callback);
        ITweenSequence OnPause(Action callback);
        ITweenSequence OnUpdate(SequenceTrackingDataEvent callback);
        ITweenSequence OnComplete(Action callback);
       
    }
}