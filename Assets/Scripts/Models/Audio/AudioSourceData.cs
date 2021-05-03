using UnityEngine;

namespace Models.Audio
{
    public struct AudioSourceData
    {
        public readonly AudioSource Source;
        private float               _value;

        public AudioSourceData(AudioSource inSource)
        {
            Source = inSource;
            _value = 0f;
        }

        public void Pause()
        {
            _value = Source.time;
            Source.Stop();
        }

        public void Resume()
        {
            Source.time = _value;
            Source.Play();
        }
    }
}