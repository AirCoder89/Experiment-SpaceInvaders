using System;
using System.Collections.Generic;
using AirCoder.TJ.Core.Ease;
using AirCoder.TJ.Core.Jobs;
using UnityEngine;

namespace AirCoder.TJ.Core.Sequences
{
    public enum SequenceType
    {
        Iteration, Parallel
    }
    
   public class SequenceJob: ITweenSequence
    {
        public float normalizedTime { get; set; }
        public bool isPlaying
        {
            get => _isPlaying;
            set => _isPlaying = value;
        }

        //events
        public event Action onPlay;
        public event Action onKill;
        public event Action onComplete;
        public event Action onPause;
        public event SequenceTrackingDataEvent onUpdate;
        
        private TJSystem _system;
        private List<ITweenJob> _jobs;
        private EaseType _selectedEase;
        private bool _isPlaying;
        private ITweenJob _currentJob;
        private SequenceType _seqType;
        private int _jobIndex;
        private float _duration;
        private bool _applyEase;
        private float _currentTime;
        private float _maxJobDuration;
        private float _totalJobDuration;
        private uint _nbLoop;
        private int _loopCounter;
        private bool _loopSequence;
        private bool _isRewind;
        private bool _playBackward;
        private SequenceTrackingData _trackingData;
        
        public SequenceJob(TJSystem system, SequenceType type)
        {
            this._system = system;
            this.Initialize(type);
        }

        public ITweenSequence Rewind()
        {
            if (_jobs.Count <= 1)
            {
                _system.LogWarning("Sequence is empty! you have to append jobs before call Rewind method.");
                return this;
            }
            _isRewind = true;
            return this;
        }
       
        public void Initialize(SequenceType sequenceType)
        {
            this._duration = 0f;
            this._totalJobDuration = 0f;
            this._maxJobDuration = 0f;
            this._loopCounter = 0;
            this._nbLoop = 0;
            ResetTime();
            _jobIndex = -1;
            _seqType = sequenceType;
            this._currentJob = null;
            _applyEase = false;
            isPlaying = false;
            _loopSequence = false;
            _isRewind = false;
            _playBackward = false;
            _jobs = new List<ITweenJob>();
            RemoveEventListeners();
            _trackingData = new SequenceTrackingData();
        }

        public List<ITweenJob> GetJobs() => this._jobs;

        public void ReleaseReferences()
        {
            throw new NotImplementedException();
        }

        private void RemoveEventListeners()
        {
            onComplete = null;
            onKill = null;
            onPause = null;
            onUpdate = null;
            onPlay = null;
        }

        public void Tick(float deltaTime)
        {
            if(!isPlaying) return;
            switch (_seqType)
            {
                case SequenceType.Iteration when _currentJob != null:
                    _currentJob.Tick(deltaTime);
                    
                    break;
                case SequenceType.Parallel when _jobs != null:
                    foreach (var tweenJob in _jobs)
                        tweenJob.Tick(deltaTime);
                    break;
            }
            
            UpdateSequenceTime(deltaTime);

            _trackingData.allJobs = GetJobs();
            _trackingData.currentJob = _currentJob;
            _trackingData.normalizedTime = this.normalizedTime;
            onUpdate?.Invoke(_trackingData); 
        }
        
        private void UpdateSequenceTime(float deltaTime)
        {
            _currentTime += deltaTime;
            var finalDuration = _duration;

            if (_duration <= 0f)
            {
                switch (_seqType)
                {
                    case SequenceType.Iteration: finalDuration = _totalJobDuration; break;
                    case SequenceType.Parallel: finalDuration = _maxJobDuration;  break;
                }
            }

            //if (_isRewind) finalDuration *= 2;
            
            if (_currentTime >= finalDuration) _currentTime = finalDuration;
            _trackingData.normalizedTime = finalDuration - _currentTime;
            normalizedTime = _currentTime / finalDuration;
            
            if(normalizedTime >= 1f && _seqType == SequenceType.Parallel) SequenceComplete(); 
        }
        
        public void Play()
        {
            switch (_seqType)
            {
                case SequenceType.Iteration:
                    if (_currentJob == null) PlayNextJob();
                    else _currentJob.Play();
                    break;
                case SequenceType.Parallel:
                    foreach (var job in _jobs)
                        job.Play();
                    break;
            }
            
            isPlaying = true;
            onPlay?.Invoke();
        }
        
        public void Pause()
        {
            isPlaying = false;
            onPause?.Invoke();
        }
        
        public void Kill()
        {
            isPlaying = false;
            onKill?.Invoke();
        }

        public ITweenSequence Append(ITweenJob nextJob)
        {
            if(_jobs == null) _jobs = new List<ITweenJob>();
            if (_seqType == SequenceType.Iteration) nextJob.OnComplete(PlayNextJob);
           
            nextJob.isBelongsToSequence = true;
            _totalJobDuration += nextJob.jobDuration;
            if (nextJob.jobDuration > this._maxJobDuration) this._maxJobDuration = nextJob.jobDuration;
            _jobs.Add(nextJob);
            if (_applyEase) nextJob.SetEase(_selectedEase);
            return this;
        }

        public ITweenSequence SetLoop(uint nbLoop)
        {
            this._loopSequence = true;
            this._nbLoop = nbLoop;
            return this;
        }

        private void PlayNextJob()
        {
            if(_playBackward) _jobIndex--;
            else  _jobIndex++;
            
            if (_jobIndex >= _jobs.Count || _jobIndex < 0)
            {
                SequenceComplete();
            }
            else
            {
                _currentJob = _jobs[_jobIndex];
                if (_playBackward)
                {
                    Debug.Log("rewind job !");
                    _currentJob.SetupRewind();
                }
                _currentJob.Play();
            }
        }

        private void SequenceComplete()
        {
            if (_isRewind && !_playBackward)
            {
                Debug.Log("Seq Complete : playBackward!");
                _playBackward = true;
                ResetTime();
                _jobIndex = _jobs.Count;
                PlayNextJob();
                return;
            }
            if (!_loopSequence) this.onComplete?.Invoke();
            else if (_loopSequence && _nbLoop == 0) PrepareForLoop();
            else if (_loopSequence && _nbLoop > 0)
            {
                _loopCounter++;
                if (_loopCounter >= _nbLoop) this.onComplete?.Invoke();
                else PrepareForLoop();
            }
        }

        private void PrepareForLoop()
        {
            Debug.Log("Prepare for loop");
            ResetTime();
            this._currentJob = null;
            isPlaying = false;
            _jobIndex = -1;
            Play();
        }

        private void ResetTime()
        {
            _trackingData.timeRemaining = 0f;
            this._currentTime = 0f;
        }
        
        public ITweenSequence SetDuration(float duration)
        {
            if (_jobs == null || _jobs.Count == 0)
            {
                _system.LogWarning("you cannot set sequence duration before appending jobs");
                return this;
            }
            this._duration = duration;
            switch (_seqType)
            {
                case SequenceType.Iteration:
                    var jobDuration = duration / _jobs.Count;
                    foreach (var job in _jobs)
                        job.SetDuration(jobDuration);
                    break;
                case SequenceType.Parallel:
                    foreach (var job in _jobs)
                        job.SetDuration(_duration);
                    break;
            }
            return this;
        }
        
        public ITweenSequence SetEase(EaseType easeType)
        {
            _applyEase = true;
            this._selectedEase = easeType;
            foreach (var job in _jobs)
                job.SetEase(_selectedEase);
            return this;
        }

        public ITweenSequence OnKill(Action callback)
        {
            this.onKill += callback;
            return this;
        }

        public ITweenSequence OnPlay(Action callback)
        {
            this.onPlay += callback;
            return this;
        }
        
        public ITweenSequence OnPause(Action callback)
        {
            this.onPause += callback;
            return this;
        }

        public ITweenSequence OnUpdate(SequenceTrackingDataEvent callback)
        {
            this.onUpdate += callback;
            return this;
        }

        public ITweenSequence OnComplete(Action callback)
        {
            this.onComplete += callback;
            return this;
        }

    }
}