using System;

namespace Models.Database
{
    [Serializable]
    public class PlayerData
    {
        public ScoreData  scores;
        public string     uid;
        public string     name;
    }
}