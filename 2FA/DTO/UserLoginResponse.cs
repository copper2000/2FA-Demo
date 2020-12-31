using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _2FA.DTO
{
    public class UserLoginResponse
    {
        public string SecretCode { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
