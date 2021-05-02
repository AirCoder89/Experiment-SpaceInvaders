using System;
using System.Collections.Generic;

namespace Models.Database
{
    [Serializable]
    public class GroupData
    {
        public List<PlayerData> players;
        public string           start;//"2014-02-09T00:00:20Z" //todo : add converter to DateTime
        public string           end;//"2060-02-19T00:00:20Z"//todo : add converter to DateTime
        public string           tournamentId;
        public int              week;

        public static GroupData RandomData()
        {
           return new GroupData()
           {
               players = new List<PlayerData>(),
               start = "2014-02-09T00:00:20Z",
               end = "2014-02-09T00:00:20Z",
               week = UnityEngine.Random.Range(1,7)
           };
        }
    }
}