using System;

namespace Models.Database
{
    [Serializable]
    public class RegisterData
    {
        public UserData  user;
        public string    refreshToken;
        public string    idToken;
    }
}