using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace _2FA.DTO
{
    public class UserLoginRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string SecretCode { get; set; }
    }
}
