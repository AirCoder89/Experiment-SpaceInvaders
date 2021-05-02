using System;

namespace Core
{
    /// <summary>
    /// By gathering all exceptions inside a single block, that will be very helpful to keep tracking
    /// all exceptions that we have throw them.
    /// </summary>
    public static class GameExceptions
    {
        public static void NullReference(string inMessage)
            => throw new NullReferenceException($"{inMessage}");
        
        public static void Exception(string inMessage)
            => throw new Exception($"{inMessage}");
        
        public static void SystemException(string inMessage)
            => throw new Exception ($"GameSystem Exception: {inMessage}");
    }
}