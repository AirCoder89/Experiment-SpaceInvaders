using System;
using System.Collections.Generic;
using AirCoder.TJ.Core.Jobs;
using AirCoder.TJ.Core.Sequences;

namespace AirCoder.TJ.Core
{
    public static class JobPool
    {
        private static Dictionary<Type, Queue<ITweenJob>> _pool;
        private static Queue<ITweenSequence> _sequencePool;

        #region Job Pool
            public static ITweenJob SpawnJob<T>()
            {
                if(_pool == null) _pool = new Dictionary<Type, Queue<ITweenJob>>();
                return _pool[typeof(T)].Dequeue();
            }
    
            public static void DespawnJob(ITweenJob job)
            {
                if(_pool == null) _pool = new Dictionary<Type, Queue<ITweenJob>>();
                if (_pool.ContainsKey(job.currentType))
                {
                    _pool[job.currentType].Enqueue(job);
                    return;
                }
                _pool.Add(job.currentType, new Queue<ITweenJob>());
                _pool[job.currentType].Enqueue(job);
            }
    
            public static bool CanSpawnJob<T>()
            {
                if(_pool == null) _pool = new Dictionary<Type, Queue<ITweenJob>>();
                return (_pool.Count > 0 && _pool.ContainsKey(typeof(T)) && _pool[typeof(T)].Count > 0);
            }
        #endregion
        
        #region Sequence Pool
            public static ITweenSequence SpawnSequence(SequenceType sequenceType)
            {
                if(_sequencePool == null) _sequencePool = new Queue<ITweenSequence>();
                var sequence =  _sequencePool.Dequeue();
                sequence?.Initialize(sequenceType);
                return sequence;
            }
        
            public static void DespawnSequence(ITweenSequence sequence)
            {
                if(_sequencePool == null) _sequencePool = new Queue<ITweenSequence>();
                _sequencePool.Enqueue(sequence);
            }
        
            public static bool CanSpawnSequence() => _sequencePool != null && _sequencePool.Count != 0;
        #endregion
    }
}