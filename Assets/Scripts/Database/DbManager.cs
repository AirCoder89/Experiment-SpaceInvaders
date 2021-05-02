using System;
using System.Collections.Generic;
using Core;
using LocalLeaderboards;
using Models.Database;
using Proyecto26;
using UnityEngine;

namespace Database
{
    public static class DbManager
    {
        private static DbConfig _config;
        private static bool _isInitialized;
        
        public static void Initialize(DbConfig inConfig)
        {
            if(_isInitialized) return;
            _isInitialized = true;
            _config = inConfig;
            if (_config.isLocal) InitLocalDataBase();
        }

        private static void InitLocalDataBase()
        {
            //fill with 2 fake leaderBoards
            LocalLeaderboardSystem.AddLeaderboardsGroup(LeaderBoardData.RandomData("DK"));
            LocalLeaderboardSystem.AddLeaderboardsGroup(LeaderBoardData.RandomData("TN"));
        }

        public static void GetToken(Action<RegisterData> onCompleted, Action<string> onFailed)
        {
            if (_config.isLocal)
            {
                var tokenResponse = LocalLeaderboardSystem.GetToken();
                if(tokenResponse.isFailed) onFailed?.Invoke(tokenResponse.message);
                else onCompleted?.Invoke(tokenResponse.result);
                return;
            }
            var request = DefaultRequest();
            request.ContentType = "application/json";
            request.Method = "GET";
            request.Uri = UriList.Auth;
            SendRequest(request, response =>
                {
                    var result = JsonUtility.FromJson<RegisterData>(response);
                    onCompleted?.Invoke(result);
                },
            onFailed);
        }

        public static void GetLeaderBoards(string inCountry, string inToken, Action<LeaderBoardData> onCompleted, Action<string> onFailed)
        {
            if (_config.isLocal)
            {
                var leaderBoardResponse = LocalLeaderboardSystem.GetLeaderboard(inCountry, inToken);
                if(leaderBoardResponse.isFailed) onFailed?.Invoke(leaderBoardResponse.message);
                else onCompleted?.Invoke(leaderBoardResponse.result);
                return;
            }
            
            var request = DefaultRequest();
            request.ContentType = "application/json";
            request.Method = "GET";
            request.Uri = UriList.Leaderboards;
            request.Headers = new Dictionary<string, string>()  { {"Authorization", $"Bearer {inToken}"}  };
            SendRequest(request, response =>
            {
                var result = JsonUtility.FromJson<LeaderBoardData>(response);
                onCompleted?.Invoke(result);
            },
            onFailed);
        }

        public static void SubmitLeaderboard(string inToken, int inPastScore, SubmitData inData, Action<PlayerData> onCompleted, Action<string> onFailed)
        {
            if (_config.isLocal)
            {
                var leaderBoardResponse = LocalLeaderboardSystem.SubmitLeaderboard(inToken, inPastScore, inData);
                if(leaderBoardResponse.isFailed) onFailed?.Invoke(leaderBoardResponse.message);
                else onCompleted?.Invoke(leaderBoardResponse.result);
                return;
            }
            
            var request = DefaultRequest();
            request.ContentType = "application/x-www-form-urlencoded";
            request.Method = "POST";
            request.Uri = UriList.LeaderboardsSubmit;
            request.Headers = new Dictionary<string, string>  { {"Authorization", $"Bearer {inToken}"}  };
            request.SimpleForm = inData.ToForm();
            SendRequest(request, response =>
            {
                var result = JsonUtility.FromJson<PlayerData>(response);
                onCompleted?.Invoke(result);
            },
            onFailed);
        }
        
        private static void SendRequest(RequestHelper request, Action<string> onCompleted, Action<string> onFailed)
        {
            if (!_isInitialized) GameExceptions.Exception($"RestClient must be initialized to send request.");
            
            Proyecto26.RestClient.Request(request)
         
                .Then(response =>
                {
                    onCompleted?.Invoke(response.Text);
                    Proyecto26.RestClient.ClearDefaultHeaders();
                })
                .Catch(err =>  onFailed?.Invoke(err.Message));
        }

        private static RequestHelper DefaultRequest()
        {
            var request = new RequestHelper()
            {
                Timeout = _config.timeOut,
                Retries = _config.retries,
                EnableDebug = _config.enableDebug,
                RetrySecondsDelay = _config.retriesDelay
            };
            request.Headers.Clear();
            return request;
        }
        
    }
}