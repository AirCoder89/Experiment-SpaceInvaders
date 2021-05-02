using UnityEngine;

namespace Models.Audio
{
    public struct AudioSourceData
    {
        public readonly AudioSource source;
        private float               _value;

        public AudioSourceData(AudioSource inSource)
        {
            source = inSource;
            _value = 0f;
        }

        public void Pause()
        {
            _value = source.time;
            source.Stop();
        }

        public void Resume()
        {
            source.time = _value;
            source.Play();
        }
    }
}