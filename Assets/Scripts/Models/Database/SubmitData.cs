using System;
using System.Collections.Generic;

namespace Models.Database
{
    [Serializable]
    public class SubmitData
    {
        public string  tournamentId;
        public string  name;
        public int     score;

        public Dictionary<string, string> ToForm()
        {
            return new Dictionary<string, string>()
            {
                {"tournamentId", tournamentId},
                {"name", name},
                {"score", score.ToString()}
            };
        }
    }
}