using System.Collections.Generic;
using Core;
using Interfaces;
using Models;
using Models.SystemConfigs;
using UnityEngine;
using Utils;
using Views;

namespace Systems
{
    public class AudioSystem : GameSystem,ITick
    {
        private static Dictionary<AudioLabel, AudioData>  _audioMap;
        private static AudioConfig                        _config;
        private static GameSystem                         _instance;
        
        public AudioSystem(SystemConfig inConfig) : base(inConfig)
        {
            if (_instance == null) _instance = this;
            if(inConfig != null) _config = inConfig as AudioConfig;

            var sourcesHolder = new GameObject("AudioSources Holder");
            sourcesHolder.transform.SetParent(LevelState.Instance.transform);
            sourcesHolder.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
            
            //- fill the audio pool
            for (var i = 0; i < _config.bufferSize; i++)
            {
                var src = sourcesHolder.AddComponent<AudioSource>();
                AudioPool.AddToPool(src);
            }
            
            //- assign audio map
            _audioMap = new Dictionary<AudioLabel, AudioData>();
            _config.audios.ForEach(audio => _audioMap.Add(audio.label, audio));
        }
        
        public override void Start()
        {
            
        }

        public static void Play(AudioLabel inAudio)
        {
            if(!_audioMap.ContainsKey(inAudio)) return;
            _audioMap[inAudio].Play(_config.masterVolume);
        }

        public static void Stop(AudioLabel inAudio)
        {
            if(!_audioMap.ContainsKey(inAudio)) return;
            _audioMap[inAudio].Stop();
        }

        public void Tick(float inDeltaTime)
        {
            if(!IsRun) return;
            foreach (var data in _audioMap)
                data.Value.Tick();
        }
    }
}