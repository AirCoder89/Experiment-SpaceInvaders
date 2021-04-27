using System;
using System.Collections.Generic;
using System.Linq;
using Systems.Physics_System.Components;
using Core;
using Interfaces;
using UnityEngine;

namespace Systems.Physics_System
{
    public class PhysicsSystem : GameSystem, IFixedTick
    {
        private readonly Dictionary<string, GameCollider> _components;

        public PhysicsSystem()
        {
            _components = new Dictionary<string, GameCollider>();
            GameView.OnAttachComponent += AttachComponent;
            GameView.OnDetachComponent += DetachComponent;
        }

        public void DetachComponent(Type inType, IComponent inComponent)
        {
            if (!inType.IsAssignableFrom(typeof(GameCollider))) return;
            if (!_components.ContainsKey(inComponent.Id))  return;
            _components.Remove(inComponent.Id);
        }

        private void AttachComponent(Type inType, IComponent inComponent)
        {
            if (!inType.IsAssignableFrom(typeof(GameCollider)))  return;
            _components.Add(inComponent.Id, (GameCollider)inComponent);
            inComponent.onDestroyed += DetachComponent;
        }

        public override void Start()
        {
            
        }

        public void FixedTick(float inFixedDeltaTime)
        {
            if(!IsRun) return;
            Debug.Log($"Physics Tick : {_components.ToList().Count}");
                foreach (var component in _components.ToList())
                    component.Value.FixedTick(inFixedDeltaTime);
        }
    }
}