using System;
using System.Collections.Generic;
using Interfaces;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Core
{
    public abstract class GameView
    {
        public static event Action<Type, IComponent> OnAttachComponent;
        public static event Action<Type, IComponent> OnDetachComponent;
        public Dictionary<Type, IComponent> Components{ get; private set; }
        private List<string> _attachedComponents;
        
        private bool _visibility;
        public bool Visibility
        {
            get => _visibility;
            set
            {
                _visibility = value;
                gameObject.SetActive(_visibility);
            }
        }
        
        public GameObject gameObject { get; private set; }
        
        public GameView(string inName)
        {
            gameObject = new GameObject(inName);
            Visibility = true;
            _attachedComponents = new List<string>();
            Components = new Dictionary<Type, IComponent>();
        }
        
        public void SetParent(Transform inParent) => gameObject.transform.parent = inParent;
        public void SetPosition(Vector3 inPosition) => gameObject.transform.localPosition = inPosition;
        
        public virtual void Destroy()
        {
            foreach (var component in Components)
            {
                Debug.Log($"remove Component {component.Key}");
                component.Value.Destroy();
            }
            
            Object.Destroy(this.gameObject);
            gameObject = null;
        }
        
        public bool HasComponent<T>() where T : IComponent
            => Components.ContainsKey(typeof(T));
        
        public T AddComponent<T>(T inComponent) where T : IComponent
        {
            if (HasComponent<T>())  return inComponent; 
            _attachedComponents.Add(typeof(T).Name);
            Components.Add(typeof(T), inComponent);
            inComponent.Attach(this);
            Debug.Log($"AddComponent<T>");
            OnAttachComponent?.Invoke(typeof(T), inComponent);
            return inComponent;
        }

        public T GetComponent<T>() where T : IComponent
        {
            if (!HasComponent<T>()) throw new NullReferenceException($"Component not found");
            var component = (T) Components[typeof(T)];
            return component;
        }
        
        public bool RemoveComponent<T>() where T : IComponent
        {
            if (!HasComponent<T>()) return false;
            var component = Components[typeof(T)];
            OnDetachComponent?.Invoke(typeof(T), component);
            _attachedComponents.Remove(typeof(T).Name);
            var res =  Components.Remove(typeof(T));
            component.Destroy();
            return res;
        }

        public void RemoveAllComponents()
        {
            _attachedComponents.Clear();
            foreach (var component in Components)
                component.Value.Destroy();
        }
    }
}
