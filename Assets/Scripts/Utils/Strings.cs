namespace Utils
{
    public static class Strings
    {
        public static string Random(int length,bool includeNumeric)
        {
            var IdChar = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            var IdNum = "0123456789";
            var val = "";
            var src = includeNumeric ? IdChar + IdNum : IdChar;
            for (var i = 0; i < length; i++)
                val += src[UnityEngine.Random.Range(0, src.Length)];
            return val;
        }
    }
}