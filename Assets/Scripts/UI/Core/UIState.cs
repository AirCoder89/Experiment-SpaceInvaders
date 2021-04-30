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
        [SerializeField] private GameStates label;
        private Canvas _canvas
        {
            get
            {
                if (_c == null) _c = GetComponent<Canvas>();
                return _c;
            }
        }

        private CanvasGroup _canvasGroup
        {
            get
            {
                if (_cg == null) _cg = GetComponent<CanvasGroup>();
                return _cg;
            }
        }

        public GameStates Label => label;
        
        public bool Visibility
        {
            get => _isVisible;
            set
            {
                _isVisible = value;
                _canvas.enabled = _isVisible;
            }
        }
        
        private CanvasGroup  _cg;
        private bool         _isVisible;
        private Canvas       _c;
        private UIManager    _manager;
        private bool         _isInitialized;
        
        public virtual void Initialize(UIManager inManager)
        {
            if(_isInitialized) return;
            _isInitialized = true;
            _manager = inManager;
            Main.RegisterGameState(this);
            CloseImmediately();
        }

        public void Open(float inDuration, Action inCallback)
        {
            Visibility = true;
            _canvasGroup.alpha = 0f;
            _canvasGroup.TweenOpacity(1f, inDuration)
                .OnComplete(inCallback)
                .Play();
        }

        public void OpenImmediately()
        {
            Visibility = true;
            _canvasGroup.alpha = 1f;
        }

        public void Close(float inDuration, Action inCallback)
        {
            Visibility = true;
            _canvasGroup.alpha = 1f;
            _canvasGroup
                .TweenOpacity(0f, inDuration)
                .OnComplete(() =>
                {
                    Visibility = false;
                    inCallback?.Invoke();
                })
                .Play();
        }

        public void CloseImmediately()
        {
            Visibility = false;
            _canvasGroup.alpha = 0f;
        }
    }
}