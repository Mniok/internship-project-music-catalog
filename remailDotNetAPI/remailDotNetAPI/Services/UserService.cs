using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using remailDotNetAPI.Models;
///using Microsoft.AspNetCore.Mvc;

namespace remailDotNetAPI.Services
{
    public interface IUserService
    {
        bool IsAnExistingUser(string userName);
        bool IsValidUserCredentials(string userName, string password);
        string GetUserRole(string userName);
        Models.User CreateUser(string userName, string password);  //notice: should be CreateUserAsync! Not async for now
    }

    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;
        private readonly DBService _context;


        private readonly IDictionary<string, string> _users = new Dictionary<string, string>            /// !!!!!!!! zast¹piæ database
        {
            { "test1", "password1" },
            { "test2", "password2" },
            { "admin", "securePassword" }
        };
        // inject your database here for user validation
        public UserService(ILogger<UserService> logger, DBService context)
        {
            _logger = logger;
            _context = context;
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
        public Models.User CreateUser(string userName, string password)
        {
            int newId = _context.User.Count() + 1;

            Models.User newUser = new Models.User(newId,
                                        userName, 
                                        password,
                                        UserRoles.BasicUser);
            //Models.User newUser = new Models.User { UserId = newId, UserName = userName, OriginalUserName = userName, Password = password, CreatedAt = DateTime.Now, UserRole = UserRoles.BasicUser };

            _context.User.Add(newUser);
            //await _context.SaveChangesAsync();
            _context.SaveChanges();

            //return CreatedAtAction("GetUser", new { id = user.UserId }, user);
            //return new Models.User(1, "testuser1", "testpass1", Services.UserRoles.BasicUser);
            return newUser;

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