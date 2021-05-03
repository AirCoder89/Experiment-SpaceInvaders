namespace LocalLeaderboards
{
    public class LocalResponse<T>
    {
        public bool    isFailed;
        public string  message;
        public T       result;
    }
}