using Systems;
using AirCoder.TJ.Core;
using AirCoder.TJ.Core.Extensions;
using AirCoder.TJ.Core.Jobs;
using Core;
using Models.SystemConfigs;
using UI.Core;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.UI_States
{
    public class PauseGameState : UIState
    {
        [SerializeField] private Image bg;
        [SerializeField] private RectTransform title;
        [SerializeField] private Vector2 startScale;
        [SerializeField] private float duration;
        [SerializeField] private EaseType ease;

        private ITweenJob _tweenJob;
        private bool _canClose;
        public override void Enter()
        {
            base.Enter();
            _canClose = false;
            Open();
        }

        private void Update()
        {
            if(!Visibility || !_canClose) return;
            if (Input.GetMouseButtonUp(0))
            {
                AudioSystem.Play(AudioLabel.Click);
                Main.ResumeGame();
                return;
            }
            /*if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                // Check if finger is over a UI element
                if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                {
                    Debug.Log("Touched the UI");
                }
            }*/
        }

        private void Open()
        {
            title.gameObject.SetActive(false);
            bg.SetAlpha(0f);
            _tweenJob = bg.TweenOpacity(0.5f, duration).OnComplete(() =>
            {
                _canClose = true;
                title.gameObject.SetActive(true);
                title.localScale = startScale;
                _tweenJob = title.TweenScale(Vector2.one, duration / 2f).SetEase(ease).Play();
            })
            .Play();
        }

        public override void Exit()
        {
            base.Exit();
            _tweenJob?.Kill();
        }
    }
}