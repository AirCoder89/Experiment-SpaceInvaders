using AirCoder.TJ.Core.Extensions;
using Core;
using Database;
using Models.Animations;
using Models.Database;
using UI.Core;
using UnityEngine;
using UnityEngine.UI;

namespace UI.UI_States
{
    public class WinningState : UIState
    {
        [SerializeField] private RectTransform  window;
        [SerializeField] private TweenData      windowAnimation;
        [SerializeField] private InputField     inputName;
        [SerializeField] private Button         submitBtn;
        [SerializeField] private Text           scoreTxt;

        public override void Initialize(UIManager inManager)
        {
            base.Initialize(inManager);
            inputName.text = string.Empty;
            submitBtn.onClick.AddListener(Submit);
            scoreTxt.text = Main.Data.Score.ToString("0000");
        }

        private void Submit()
        {
            if(string.IsNullOrEmpty(inputName.text)) return;
                submitBtn.interactable = false;
                var data = Main.Data;
                DbManager.SubmitLeaderboard(data.Register.idToken, 0, new SubmitData()
                {
                    name = inputName.text,
                    score = Main.Data.Score,
                    tournamentId = data.Leaderboards.@group.tournamentId
                }, 
                 LeaderboardsSubmited,
                 OnSubmitFailed);
        }

        private void OnSubmitFailed(string inMessage)
        {
            submitBtn.interactable = true;
            Debug.LogError($"Submit Failed : {inMessage}");
        }

        private void LeaderboardsSubmited(PlayerData inData)
        {
            StateManager.UpdateGameState(States.Menu);
        }
        
        public override void Enter()
        {
            base.Enter();
            submitBtn.interactable = true;
            window.localScale = windowAnimation.target;
            FadeIn(() => { window.TweenScale(Vector2.one, windowAnimation.duration).SetEase(windowAnimation.ease).Play(); });
        }
    }
}
