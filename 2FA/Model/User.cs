using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _2FA.Model
{
    public class User
    {
        [System.ComponentModel.DataAnnotations.Key]
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set;}
        public string SecretCode { get; set; }
    }
}
