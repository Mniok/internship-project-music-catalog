using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace remailDotNetAPI.Services
{
    public interface IUserService
    {
        bool IsAnExistingUser(string userName);
        bool IsValidUserCredentials(string userName, string password);
        string GetUserRole(string userName);
        bool CreateUser(string userName, string password);  //notice: should be CreateUserAsync! Not async for now
    }

    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;


        private readonly IDictionary<string, string> _users = new Dictionary<string, string>            /// !!!!!!!! zast¹piæ database
        {
            { "test1", "password1" },
            { "test2", "password2" },
            { "admin", "securePassword" }
        };
        // inject your database here for user validation
        public UserService(ILogger<UserService> logger)
        {
            _logger = logger;
        }

        public bool IsValidUserCredentials(string userName, string password)
        {
            _logger.LogInformation($"Validating user [{userName}]");
            if (string.IsNullOrWhiteSpace(userName))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                return false;
            }

            return _users.TryGetValue(userName, out var p) && p == password;
        }

        public bool IsAnExistingUser(string userName)
        {
            return _users.ContainsKey(userName);
        }

        public string GetUserRole(string userName)
        {
            if (!IsAnExistingUser(userName))
            {
                return string.Empty;
            }

            /*if (userName == "admin")      /// !!! takie by³o u Changhui Xu. Dobrze ¿e zauwa¿y³em i zakomentowa³em bo mog³oby to byæ vulnerability
            {
                return UserRoles.Admin;
            }*/

            return UserRoles.BasicUser;
        }



        ///     !!!     nowe: CreateUserAsync do rejestracji ze stackoverflowa, bo u Changhui'a Xu rejestracji z jakiegoœ powodu nie by³o
        public bool CreateUser(string userName, string password)
        {
            //var user = _mapper.Map<User>(userRegistration);
            //var result = await _userManager.CreateAsync(user, userRegistration.Password);
            //return true; //success
            return false; //failure     //TEMPORARY: just for testing always fails, remove this later 
        }

        /* rejestracja od Hadeeba Ajide (czêœæ):
        public async Task<IdentityResult> RegisterUserAsync(UserRegistrationDto userRegistration)
        {
            var user = _mapper.Map<User>(userRegistration);
            var result = await _userManager.CreateAsync(user, userRegistration.Password);
            return result;
        }
        */


    }

    public static class UserRoles
    {
        public const string Admin = nameof(Admin);
        public const string BasicUser = nameof(BasicUser);
    }
}