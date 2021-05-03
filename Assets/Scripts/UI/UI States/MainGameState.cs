using Systems;
using Core;
using Models.SystemConfigs;
using UI.Core;
using UnityEngine;
using UnityEngine.UI;

namespace UI.UI_States
{
    public class MainGameState : UIState
    {
        [SerializeField] private RectTransform  title;
        [SerializeField] private float          floatStrength = 1; 
        [SerializeField] private float          speed ; 
        [SerializeField] private Button         startBtn;
        [SerializeField] private Button         leaderBoardBtn;

        private float _originalY;
        
        public override void Initialize(UIManager inManager)
        {
            base.Initialize(inManager);
            startBtn.onClick.AddListener(StartNewGame);
            leaderBoardBtn.onClick.AddListener(GoToLeaderBoardState);
            _originalY = title.anchoredPosition.y;
        }

        public override void Enter()
        {
            base.Enter();
            FadeIn();
        }

        public override void Exit()
        {
            base.Exit();
            FadeOut();
        }

        public override void Tick(float inDeltaTime)
        {
            base.Tick(inDeltaTime);
            title.anchoredPosition = new Vector2(title.anchoredPosition.x, _originalY + ((float)Mathf.Sin(Time.time * speed) * floatStrength));
        }

        private void GoToLeaderBoardState()
        {
            AudioSystem.Play(AudioLabel.Click);
            StateManager.UpdateGameState(States.LeaderBoards);
        }

        private void StartNewGame()
        {
            AudioSystem.Play(AudioLabel.Click);
            Main.StartNewGame();
        }
    }
}