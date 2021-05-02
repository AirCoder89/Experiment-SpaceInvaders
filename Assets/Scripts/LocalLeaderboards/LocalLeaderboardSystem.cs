using System;
using System.Collections.Generic;
using System.Linq;
using AirCoder.NaughtyAttributes.Scripts.Core.ValidatorAttributes;
using Models.Database;
using UnityEngine;

namespace LocalLeaderboards
{
    public class LocalLeaderboardSystem : MonoBehaviour
    {
        private static LocalLeaderboardSystem _instance;
        [SerializeField] [Required] private LocalDatabase database;

        private void Awake()
        {
            if(_instance != null) return;
            _instance = this;
        }

        public static LocalResponse<RegisterData> GetToken()
        {
            if (_instance.database.Tokens == null)
            {
                _instance.database.Tokens = new Dictionary<string, int>();
                Debug.Log($"Assign token");
            }
            var token = Guid.NewGuid().ToString("N");
            var userId = _instance.database.Tokens.Count;
            _instance.database.Tokens.Add(token, userId);
            return new LocalResponse<RegisterData>()
            {
                result = new RegisterData()
                {
                    user = new UserData(){id = userId.ToString()},
                    idToken = token,
                    refreshToken = Guid.NewGuid().ToString("N")
                }
            };
        }

        public static LocalResponse<LeaderBoardData> GetLeaderboard(string inCountry, string inToken)
        {
            if(!_instance.database.Tokens.ContainsKey(inToken)) return new LocalResponse<LeaderBoardData>()
            {
                isFailed = true,
                message = "Invalid Token"
            };
            if(_instance.database.LeaderBoards == null || _instance.database.LeaderBoards.Count == 0)return new LocalResponse<LeaderBoardData>()
            {
                isFailed = true,
                message = "Empty table"
            };
            if(!_instance.database.LeaderBoards.ContainsKey(inCountry))return new LocalResponse<LeaderBoardData>()
            {
                isFailed = true,
                message = "Not found"
            };
            return new LocalResponse<LeaderBoardData>()
            {
                result = _instance.database.LeaderBoards[inCountry]
            };
        }
        
        public static void AddLeaderboardsGroup(LeaderBoardData inGroup)
        {
            if (_instance.database.LeaderBoards == null) _instance.database.LeaderBoards = new Dictionary<string, LeaderBoardData>();
            _instance.database.LeaderBoards.Add(inGroup.country, inGroup);
        }

        public static LocalResponse<PlayerData> SubmitLeaderboard(string inToken, int inPast, SubmitData inData)
        {
            if(!_instance.database.Tokens.ContainsKey(inToken)) return new LocalResponse<PlayerData>()
            {
                isFailed = true,
                message = "Invalid Token"
            };
            if(_instance.database.LeaderBoards == null || _instance.database.LeaderBoards.Count == 0)return new LocalResponse<PlayerData>()
            {
                isFailed = true,
                message = "Empty table"
            };
            var leaderboard = GetLeaderBoardByTournamentId(inData.tournamentId);
            if(leaderboard == null) return new LocalResponse<PlayerData>()
            {
                isFailed = true,
                message = "Group Not found"
            };

            var newPlayerData = new PlayerData()
            {
                uid = _instance.database.Tokens[inToken].ToString(),
                name = inData.name,
                scores = new ScoreData()
                {
                    current = inData.score,
                    past = inPast
                }
            };
            leaderboard.group.players.Add(newPlayerData);
            
            return new LocalResponse<PlayerData>()
            {
                result = newPlayerData
            };
        }

        private static LeaderBoardData GetLeaderBoardByTournamentId(string inTournamentId)
        {
            return  _instance.database.LeaderBoards.Values.FirstOrDefault(l => l.@group.tournamentId == inTournamentId);
        }

        private void OnDestroy()
        {
            database.Tokens?.Clear(); //release session's tokens
        }
    }
}