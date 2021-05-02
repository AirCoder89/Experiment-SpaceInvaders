namespace Database
{
    public static class UriList
    {
        public static string BaseUrl  => "v1";
        public static string Auth => $"{BaseUrl}/auth/register";
        public static string Leaderboards => $"{BaseUrl}/leaderboards";
        public static string LeaderboardsSubmit => $"{Leaderboards}/submit";
    }
}