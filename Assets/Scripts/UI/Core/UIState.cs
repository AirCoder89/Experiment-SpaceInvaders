using System;
using AirCoder.TJ.Core.Extensions;
using Core;
using Interfaces;
using UnityEngine;

namespace UI.Core
{
    [RequireComponent(typeof(Canvas))]
    [RequireComponent(typeof(CanvasGroup))]
    public class UIState : MonoBehaviour, IGameState
    {
        [SerializeField] private States label;
        private Canvas _rootCanvas
        {
            get
            {
                if (_canvas == null) _canvas = GetComponent<Canvas>();
                return _canvas;
            }
        }
        public CanvasGroup _canvasGroup
        {
            get
            {
                if (_cGroup == null) _cGroup = GetComponent<CanvasGroup>();
                return _cGroup;
            }
        }
        public States Label => label;
        
        public bool Visibility
        {
            get => _isVisible;
            private set
            {
                _isVisible = value;
                _rootCanvas.enabled = _isVisible;
            }
        }

        private bool         _isVisible;
        private Canvas       _canvas;
        private CanvasGroup  _cGroup;
        private UIManager    _manager;
        private bool         _isInitialized;
        
        public virtual void Initialize(UIManager inManager)
        {
            if(_isInitialized) return;
            _isInitialized = true;
            _manager = inManager;
            StateManager.RegisterGameState(this);
            Visibility = false;
        }

        public virtual void Enter()
        {
            Visibility = true;
        }

        public virtual void Tick(float inDeltaTime)
        {
            
        }

        public virtual void Exit()
        {
            Visibility = false;
        }

        public void FadeIn(Action inCallback = null)
        {
            _canvasGroup.alpha = 0f;
            _canvasGroup.TweenOpacity(1f, _manager.fadeTransitionDuration).OnComplete(inCallback).Play();
        }
        
        public void FadeOut(Action inCallback = null)
        {
            _canvasGroup.alpha = 1f;
            _canvasGroup.TweenOpacity(0f, _manager.fadeTransitionDuration).OnComplete(inCallback).Play();
        }
    }
}