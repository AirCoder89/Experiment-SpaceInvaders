using System.Collections.Generic;
using System.Linq;
using Core;
using Database;
using Models.Database;
using UI.Core;
using UI.Leaderboards;
using UnityEngine;

namespace UI.UI_States
{
    public class LeaderboardsState : UIState
    {
        [SerializeField] private LeaderBoardItem   itemPrefab;
        [SerializeField] private RectTransform     itemsHolder;

        private List<LeaderBoardItem> _items;

        public override void Initialize(UIManager inManager)
        {
            base.Initialize(inManager);
            if (Main.Data.Register == null)
                DbManager.GetToken(AssignToken, OnGetTokenFailed);
        }

        private void AssignToken(RegisterData inData)
        {
            Main.Data.Register = inData;
        }

        private void OnGetTokenFailed(string inMessage)
        {
            Debug.LogError($"Failed to get token : {inMessage}");
            StateManager.UpdateGameState(States.Menu);
        }

        public override void Enter()
        {
            DbManager.GetLeaderBoards(Main.Settings.currentCountry, Main.Data.Register.idToken, AssignLeaderboard, OnGetLeaderboardFailed); 
            base.Enter();
            FadeIn();
        }

        private void AssignLeaderboard(LeaderBoardData inLeaderBoards)
        {
            Main.Data.Leaderboards = inLeaderBoards;
            Main.Data.Leaderboards.CalculateLastRank();
            UpdateLeaderBoards();
        }

        private void OnGetLeaderboardFailed(string inMessage)
        {
            Debug.LogError($"Failed to get Leaderboards : {inMessage}");
            StateManager.UpdateGameState(States.Menu);
        }

        private void UpdateLeaderBoards()
        {
            if(Main.Data.Leaderboards == null || Main.Data.Leaderboards.group == null) return;
            Clear();
            var finalList = Main.Data.Leaderboards.group.players.OrderByDescending(g => g.scores.current).ToList();
            for (var i = 0; i < finalList.Count; i++)
            {
                if(i >= 10) return;
                var playerData = finalList[i];
                if (i >= _items.Count)
                {
                    var newItem = Instantiate(itemPrefab, itemsHolder);
                    _items.Add(newItem);
                }
                _items[i].BindData(i+1, playerData.name, playerData.scores.current);
            }
        }
        
        public override void Exit()
        {
            base.Exit();
            FadeOut();
        }

        private void Clear()
        {
            if(_items == null) _items = new List<LeaderBoardItem>();
            foreach (var item in _items)
                item.Clear();
        }
    }
}