using System;
using System.ComponentModel;
using AirCoder.TJ.Core.Ease;
using UnityEngine;

namespace AirCoder.TJ.Core.Jobs
{
   public abstract class TweenJobBase : ITweenJob
    {
        //- events
        public event Action onKill;
        public event Action onComplete;
        public event Action onPlay;
        public event Action onPause;
        public event Action onRewind;
        public event Action onResume;
        public event onUpdateEvent onUpdate;
        
        //- properties
        public float normalizedTime { get; set; }

        public float jobDuration
        {
            get => duration;
            set => duration = value;
        }

        public bool isPlaying {
            get => IsPlaying;
            set => IsPlaying = value;
        }

        public bool isBelongsToSequence { get; set; }
        public Type currentType { get; set; }

        //- protected
        protected Easing.Function selectedEase;
        protected float currentTime;
        protected bool IsPlaying;
        protected float duration;
        protected JObType currentJobType;
        protected Action jobAction;
        
        private bool _isRewind;
        private float _remainingTime;
        private JobTrackingData _trackingData;
        
        public void Initialize(Type type)
        {
            currentType = type;
            _trackingData = new JobTrackingData();
        }

        public abstract void SetupRewind();
        public abstract ITweenJob TweenTo<T>(T targetInstance, JObType job, params object[] parameters);

        public abstract void Tick(float deltaTime);
        
        public ITweenJob Play(bool rewind = false)
        {
            if(isPlaying) return this;
            currentTime = 0f;
            _remainingTime = duration;
            _isRewind = rewind;
            jobAction?.Invoke();
            isPlaying = true;
            onPlay?.Invoke();
            return this;
        }

        public void Pause()
        {
            isPlaying = false;
            onPause?.Invoke();
        }
        
        public void Resume()
        {
            isPlaying = true;
            onResume?.Invoke();
        }

        public void Kill()
        {
            isPlaying = false;
            currentTime = 0f;
            onKill?.Invoke();
        }

        public void Reset()
        {
            RemoveEventListeners();
            currentTime = 0f;
            isPlaying = false;
            isBelongsToSequence = false;
        }

        public void SetDuration(float inDuration)
        {
            this.duration = inDuration;
        }

        public virtual void ReleaseReferences()
        {
            //TODO : complete the reset
            RemoveEventListeners();
        }
        
        private void RemoveEventListeners()
        {
            onComplete = null;
            onKill = null;
            onPause = null;
            onRewind = null;
            onUpdate = null;
            onPlay = null;
            onResume = null;
        }
        
        #region Event Invoker
            protected void RaiseOnTick()
            {
                _trackingData.timeRemaining = duration - currentTime;
                _trackingData.normalizedTime = normalizedTime;
                onUpdate?.Invoke(_trackingData);
            }
            protected void RaiseOnComplete()
            {
                currentTime = 0f;
                isPlaying = false;
                onComplete?.Invoke();
            }
            protected void RaiseOnPlay()
            {
                onPlay?.Invoke();
            }
            protected void RaiseOnKill()
            {
                onKill?.Invoke();
            }
        #endregion
        
        #region Helper Methods

         protected void CheckJobInterpolationComplete()
         {
             if (this.normalizedTime >= 1f)
             {
                 this.normalizedTime = 1f;
                 if (!_isRewind)
                 {
                     IsPlaying = false;
                     RaiseOnComplete();
                 }
                 else
                 {
                     onRewind?.Invoke();
                     SetupRewind();
                     _isRewind = false;
                     this.normalizedTime = 0f;
                     this.currentTime = 0f;
                     jobAction?.Invoke();
                 }
             }
         }
            protected Color TweenColor(Color from, Color to, float time, float speed)
            {
                return new Color(
                    InterpolateFloat(from.r, to.r, time, speed),
                    InterpolateFloat(from.g, to.g, time, speed),
                    InterpolateFloat(from.b, to.b, time, speed),
                    InterpolateFloat(from.a, to.a, time, speed)
                );
            }
            
            protected Vector3 TweenVector3(Vector3 from, Vector3 to, float time, float speed)
            {
                return new Vector3(
                    InterpolateFloat(from.x, to.x, time, speed),
                    InterpolateFloat(from.y, to.y, time, speed),
                    InterpolateFloat(from.z, to.z, time, speed)
                );
            }
            
            protected Vector2 TweenVector2(Vector2 from, Vector2 to, float time, float speed)
            {
                return new Vector2(
                    InterpolateFloat(from.x, to.x, time, speed),
                    InterpolateFloat(from.y, to.y, time, speed)
                );
            }
            
            protected float InterpolateFloat(float from, float to, float time, float speed)
            {
                if(selectedEase == null) throw new Exception("SelectedEase is null");
                return selectedEase(time, from, to, speed);
            }

            protected void ThrowInvalidJobType()
            {
                throw new InvalidEnumArgumentException($"CurrentJobType", (int)currentJobType, typeof(JObType));
            }

            protected void ThrowMissingReferenceException<T>(T targetType)
            {
                throw new MissingReferenceException($"{typeof(T).Name} it's no a valid type");
            }
        #endregion
        
        #region Chained Methods
        
            public ITweenJob OnKill(Action callback)
            {
                this.onKill += callback;
                return this;
            }
    
            public ITweenJob OnPlay(Action callback)
            {
                this.onPlay += callback;
                return this;
            }
    
            public ITweenJob OnUpdate(onUpdateEvent callback)
            {
                this.onUpdate += callback;
                return this;
            }
    
            public ITweenJob OnComplete(Action callback)
            {
                this.onComplete += callback;
                return this;
            }
            
            public ITweenJob OnComplete(ITweenJob nextJob, bool rewind = false)
            {
                if (nextJob == null) throw new NullReferenceException($"next tweenJob must be not null!");
                return OnComplete(() => { nextJob.Play(rewind); });
            }
            
            public ITweenJob SetEase(EaseType easeType)
            {
                this.selectedEase = Easing.GetEase(easeType);
                return this;
            }
            public ITweenJob OnPause(Action callback)
            {
                this.onPause += callback;
                return this;
            }
            public ITweenJob OnResume(Action callback)
            {
                this.onResume += callback;
                return this;
            }
            public ITweenJob OnRewind(Action callback)
            {
                this.onRewind += callback;
                return this;
            }
        #endregion
    }
}