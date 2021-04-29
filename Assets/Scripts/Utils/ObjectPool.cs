using System;
using System.Collections.Generic;
using Core;
using Interfaces;

namespace Utils
{
    public static class ObjectPool
    {
        private static Dictionary<Type, Queue<GameView>> _objectMap;

        public static void AddToPool<T>(List<GameView> inObjects) where T : GameView, IPoolObject
        {
            if (_objectMap == null) _objectMap = new Dictionary<Type, Queue<GameView>>();
            if(!_objectMap.ContainsKey(typeof(T))) _objectMap.Add(typeof(T), new Queue<GameView>());
            inObjects.ForEach(obj =>_objectMap[typeof(T)].Enqueue(obj));
        }
        
        public static void AddToPool<T>(GameView inObject) where T : GameView, IPoolObject
        {
            if (_objectMap == null) _objectMap = new Dictionary<Type, Queue<GameView>>();
            if(!_objectMap.ContainsKey(typeof(T))) _objectMap.Add(typeof(T), new Queue<GameView>());
            _objectMap[typeof(T)].Enqueue(inObject);
        }

        private static void AutoDespawnObject(Type inType, GameView inObj)
        {
            _objectMap[inType].Enqueue(inObj);
        }

        public static T GetObject<T>() where T : GameView, IPoolObject
        {
            if (!HasObject<T>())  return null;
            if (_objectMap[typeof(T)].Dequeue() is T target)
            {
                target.onDspawn += AutoDespawnObject;
                return target;
            }
            return null;
        }

        private static bool HasObject<T>() where T : GameView, IPoolObject
        {
            if (_objectMap == null || _objectMap.Count == 0) return false;
            return _objectMap.ContainsKey(typeof(T)) && _objectMap[typeof(T)] != null && _objectMap[typeof(T)].Count != 0;
        }
    }
}