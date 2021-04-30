using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    public static class AudioPool
    {
        private static Queue<AudioSource> _sourcesBuffer;

        public static void AddToPool(AudioSource inSource)
        {
            if (_sourcesBuffer == null) _sourcesBuffer = new Queue<AudioSource>();
            _sourcesBuffer.Enqueue(inSource);
        }
        
        public static AudioSource GetSource()
        {
            return _sourcesBuffer.Dequeue();
        }
    }
}