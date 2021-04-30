using System;
using System.Collections.Generic;
using System.Linq;
using Models.SystemConfigs;
using UnityEngine;
using Utils;

namespace Models
{
    [Serializable]
    public class AudioData
    {
        public AudioLabel         label;
        public AudioClip          clip;
        [Range(0,1)] public float volume;
        public float              pitch;
        public bool               loop;

        private HashSet<AudioSource> _sources;
        
        public void Play(float inMasterVolume)
        {
            var source = AssignClipData(AudioPool.GetSource());
            if (_sources == null)
            {
                Debug.Log($"Assign new sourcec");
                _sources = new HashSet<AudioSource>();
            }
            source.volume = this.volume * inMasterVolume;
            source.Play();
            _sources.Add(source);
        }

        private AudioSource AssignClipData(AudioSource inSource)
        {
            if (inSource == null)
                throw new NullReferenceException($"Audio Source is null! you have to increase the pool size.");
            
            inSource.clip = this.clip;
            inSource.pitch = this.pitch;
            inSource.loop = this.loop;
            return inSource;
        }

        public void Tick()
        {
            if(_sources == null || _sources.Count == 0) return;
           
            var completedSrc = _sources.Where(src => !src.isPlaying && !src.loop);
            foreach (var audioSource in completedSrc.ToList())
            {
                AudioPool.AddToPool(audioSource);
                _sources.Remove(audioSource);
            }
        }

        public void Stop()
        {
            if(_sources == null || _sources.Count == 0) return;
            foreach (var source in _sources)
                source.Stop();
        }

    }
}