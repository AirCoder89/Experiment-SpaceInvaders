using System;

namespace Database
{
    [Serializable]
    public class DbConfig
    {
        public bool  isLocal;
        public int   timeOut;
        public int   retries;
        public int   retriesDelay;
        public bool  enableDebug;
    }
}