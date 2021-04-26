using System;
using AirCoder.TJ.Core.Ease;

namespace AirCoder.TJ.Core.Jobs
{
    [System.Serializable]
    public struct JobTrackingData
    {
        public float normalizedTime;
        public float timeRemaining;
    }
    
    public enum JObType
    {
        Config, Scale, Position, Rotation, Opacity, Color, FillAmount, Offset, Size, LocalPosition, LocalRotation
    }
    public delegate void onUpdateEvent(JobTrackingData jobData); 
    public interface ITweenJob
    {
        float normalizedTime { get; set; }
        float jobDuration { get; set; }
        bool isPlaying { get; set; }
        bool isBelongsToSequence { get; set; }
        Type currentType { get; set; }
        
        void Initialize(Type type);
        ITweenJob TweenTo<T>(T targetInstance,JObType job, params object[] parameters);
        void Tick(float deltaTime);
        ITweenJob Play(bool rewind = false);
        void Resume();
        void Pause();
        void Kill();
        void Reset();
        void SetDuration(float duration);

        void SetupRewind();
        //chained events subscribe
        ITweenJob SetEase(EaseType easeType);
        ITweenJob OnKill(Action callback);
        ITweenJob OnRewind(Action callback);
        ITweenJob OnPlay(Action callback);
        ITweenJob OnPause(Action callback);
        ITweenJob OnResume(Action callback);
        ITweenJob OnUpdate(onUpdateEvent callback);
        ITweenJob OnComplete(Action callback);
        ITweenJob OnComplete(ITweenJob nextJob, bool rewind = false);
    }

}