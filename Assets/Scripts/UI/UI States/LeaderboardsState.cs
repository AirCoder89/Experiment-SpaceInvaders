using UI.Core;
using UnityEngine;

namespace UI.UI_States
{
    public class LeaderboardsState : UIState
    {
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
    }
}