using System;
using System.Linq;
using _2FA.Data;
using _2FA.DTO;
using Microsoft.AspNetCore.Mvc;

namespace _2FA.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class _2FAController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly Random _random = new Random();

        public _2FAController(DataContext context)
        {
            _context = context;
        }

        [HttpPost("authenticate/2fa")]
        public UserLoginResponse Authenticate(UserLoginRequest request)
        {
            var response = new UserLoginResponse();
            
            var user = _context.Users.FirstOrDefault(x => x.UserName.ToLower().Equals(request.UserName.ToLower()));

            if (user == null)
            {
                response.Success = false;
                response.Message = "User not found";
                return response;
            } 
            
            if (!user.Password.Equals(request.Password))
            {
                response.Success = false;
                response.Message = "Password incorrect";
                return response;
            }

            if (user.SecretCode != null)
            {
                // redirect to submit page
                // user enter the secret code 
                // check with db

                response.SecretCode = user.SecretCode;
                response.Success = true;
                response.Message = "Redirect to submit page";

                return response;
            }

            var secretCode = GenerateSecretKey();
            response.Success = true;
            response.Message = "Secret Code is created";
            response.SecretCode = secretCode;

            // save new secret code to db
            user.SecretCode = secretCode;
            _context.Users.Update(user);
            _context.SaveChanges();

            return response;
        }

        public string GenerateSecretKey()
        {
            return _random.Next(1, 10).ToString();
        }

        [HttpPost("authenticate/submit")]
        public UserLoginResponse Submit(UserLoginRequest request)
        {
            var response = new UserLoginResponse();

            var user = _context.Users.FirstOrDefault(x => x.UserName.ToLower().Equals(request.UserName.ToLower()));

            if (user?.SecretCode != null)
            {
                var valid = VerifyKey(user.SecretCode, request.SecretCode);
                if (valid)
                {
                    // redirect to default page
                    response.Success = true;
                    response.Message = "Login success";
                    return response;
                }
                
                response.Success = false;
                response.Message = "Redirect to submit page";
                return response;
            }

            // redirect to login page
            return null;
        }

        public bool VerifyKey(string userSecretCode, string requestSecretCode)
        {
            if (userSecretCode.Equals(requestSecretCode))
            {
                return true;
            }

            return false;
        }

    }
}
