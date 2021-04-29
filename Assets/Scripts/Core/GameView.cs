using System;
using System.Collections.Generic;
using UnityEngine;
using Utils;
using Object = UnityEngine.Object;

namespace Core
{
    public abstract class GameView
    {
        public Vector3 Position => gameObject.transform.localPosition;
        
        private Dictionary<Type, Component> _components;
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
        public Transform transform { get; private set; }
        
        public GameView(string inName)
        {
            gameObject = new GameObject(inName);
            transform = gameObject.transform;
            Visibility = true;
            _components = new Dictionary<Type, Component>();
            ObjectMap.Subscribe(gameObject.GetInstanceID(), this);
        }
        
        public void SetParent(Transform inParent) => gameObject.transform.parent = inParent;
        public void SetPosition(Vector3 inPosition) => gameObject.transform.localPosition = inPosition;
        public void SetScale(Vector3 inScale) => gameObject.transform.localScale = inScale;
        
        public virtual void Destroy()
        {
            RemoveAllComponents();
            ObjectMap.Unsubscribe(gameObject.GetInstanceID());
            Object.Destroy(this.gameObject);
            gameObject = null;
        }
        
        public bool HasComponent<T>() where T : Component
            => _components.ContainsKey(typeof(T));
        
        public T AddComponent<T>() where T : Component
        {
            if (HasComponent<T>())  return _components[typeof(T)] as T;
            var newComponent = gameObject.AddComponent<T>();
            _components.Add(typeof(T), newComponent);
            return newComponent;
        }

        public T GetComponent<T>() where T : Component
        {
            if (!HasComponent<T>()) throw new NullReferenceException($"Component not found");
            var component = (T) _components[typeof(T)];
            return component;
        }
        
        public bool RemoveComponent<T>() where T : Component
        {
            if (!HasComponent<T>()) return false;
            var component = _components[typeof(T)];
            var res =  _components.Remove(typeof(T));
            Object.Destroy(component);
            return res;
        }

        public void RemoveAllComponents()
        {
            foreach (var component in _components)
                Object.Destroy(component.Value);
        }
    }
}
