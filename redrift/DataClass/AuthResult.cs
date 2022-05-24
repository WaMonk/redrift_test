using System;

namespace redrift.DataClass
{
    public class AuthResult : IAuthResult
    {

        public uint UserId { get; }
        public string Token { get; }

        public AuthResult(uint uid, string token )
        {
            UserId = uid;
            Token = token;
        }


    }
}

