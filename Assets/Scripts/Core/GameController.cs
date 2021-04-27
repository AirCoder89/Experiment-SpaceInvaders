using System;
using System.Collections.Generic;
using Interfaces;
using UnityEngine;

namespace Core
{
    public class GameController
    {
        private readonly Dictionary<Type, GameSystem> _systems;

        private bool _isRun;
        
        public GameController()
        {
            _systems = new Dictionary<Type, GameSystem>();
        }

        public GameController AddSystem(GameSystem inSystem)
        {
            if(_systems.ContainsKey(inSystem.GetType())) throw new Exception($"Cannot duplicate game systems");
            _systems.Add(inSystem.GetType(), inSystem);
            return this;
        }
        
        public T GetSystem<T>() where T : GameSystem
        {
            if (!_systems.ContainsKey(typeof(T))) return null;
            return (T) _systems[typeof(T)];
        }
    
        public void Tick(float inDeltaTime)
        {
            if(!_isRun) return;
            foreach (var system in _systems.Values)
                if(system is ITick systemTick) systemTick?.Tick(inDeltaTime); 
        }
        
        public void FixedTick(float inFixedDeltaTime)
        {
            if(!_isRun) return;
            foreach (var system in _systems.Values)
                if(system is IFixedTick systemFixedTick) systemFixedTick?.FixedTick(inFixedDeltaTime); 
        }

        public void Start()
        {
            _isRun = true;
            foreach (var system in _systems.Values)
                system.Start();
        }
    
        public void Pause()
        {
            foreach (var system in _systems.Values)
                system.Pause();
        }
        
        public void Resume()
        {
            foreach (var system in _systems.Values)
                system.Resume();
        }
    }
}
