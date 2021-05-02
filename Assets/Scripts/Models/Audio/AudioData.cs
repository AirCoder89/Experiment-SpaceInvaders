using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Models.SystemConfigs;
using UnityEngine;
using Utils;

namespace Models.Audio
{
    [Serializable]
    public class AudioData
    {
        public AudioLabel         label;
        public AudioClip          clip;
        [Range(0,1)] public float volume;
        public float              pitch;
        public bool               loop;
        public bool               singleSource;
        
        private HashSet<AudioSourceData> _sources;
        
        public void Play(float inMasterVolume)
        {
            var source = AssignClipData(GetSource());
            if (_sources == null) _sources = new HashSet<AudioSourceData>();
            source.volume = this.volume * inMasterVolume;
            source.Play();
            _sources.Add(new AudioSourceData(source));
        }

        private AudioSource GetSource()
        {
            if (!singleSource) return AudioPool.GetSource();
                if (_sources == null || _sources.Count == 0) return AudioPool.GetSource();
                return _sources.First().source;
        }

        private AudioSource AssignClipData(AudioSource inSource)
        {
            if (inSource == null)
                GameExceptions.NullReference($"Audio Source is null! you have to increase the pool size.");
            
            inSource.clip = this.clip;
            inSource.pitch = this.pitch;
            inSource.loop = this.loop;
            return inSource;
        }

        public void Tick()
        {
            if(_sources == null || _sources.Count == 0) return;
           
            var completedSrc = _sources.Where(src => !src.source.isPlaying && !src.source.loop);
            foreach (var audioSource in completedSrc.ToList())
            {
                AudioPool.AddToPool(audioSource.source);
                _sources.Remove(audioSource);
            }
        }

        public void Pause()
        {
            if(_sources == null || _sources.Count == 0) return;
            foreach (var audioSource in _sources.ToList())
                audioSource.Pause();
        }

        public void Resume()
        {
            if(_sources == null || _sources.Count == 0) return;
            foreach (var audioSource in _sources.ToList())
                audioSource.Resume();
        }

        public void Stop()
        {
            if(_sources == null || _sources.Count == 0) return;
            foreach (var source in _sources)
                source.source.Stop();
        }

    }
}