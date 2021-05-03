using System.Collections.Generic;
using Models.Database;
using UnityEngine;

namespace LocalLeaderboards
{
    [CreateAssetMenu(menuName = "Game/Local db")]
    public class LocalDatabase : ScriptableObject
    {
        public Dictionary<string, int>             Tokens { get; set; } //key: token | value : userId
        public Dictionary<string, LeaderBoardData> LeaderBoards { get; set; } //key: country | value : LeaderBoardData
    }
}