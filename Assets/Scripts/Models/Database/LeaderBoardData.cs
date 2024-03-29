﻿using System;
using System.Linq;

namespace Models.Database
{
    [Serializable]
    public class LeaderBoardData
    {
        public GroupData group;
        public string    country;
        public int       lastRankScore; // use it to compare with the current player score

        public static LeaderBoardData RandomData(string inCountry) => new LeaderBoardData()
        {
            lastRankScore = 0,
            country = inCountry,
            group = GroupData.RandomData()
        };

        public void CalculateLastRank()
        {
            if (group.players == null || group.players.Count == 0)
            {
                lastRankScore = 0;
                return;
            }
            lastRankScore = group.players.Min(score => score.scores.current);
        }
    }
}