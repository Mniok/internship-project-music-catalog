using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using musicCatalogDotNetAPI.Models;
///using Microsoft.AspNetCore.Mvc;

namespace musicCatalogDotNetAPI.Services
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

            //find by username and pass in db, check if one user was found (can't be more than one)
            var foundUsers = from user in _context.User.ToList() where user.UserName == userName && user.Password == password select user;

            if (foundUsers.Count() > 1)
                _logger.LogCritical($"User [{userName}] cound not be validated, because somehow more than one user with this username exists!");

            return foundUsers.Count() == 1; //jak 0 to nie ma takiego, >1 nie mo¿e byæ bez bezpoœredniego grzebania w bazie
        }

        public bool IsAnExistingUser(string userName)
        {
            //return _users.ContainsKey(userName);
            //_context.User.Find(var x => x.UserName == userName);
            var foundUsers = from user in _context.User.ToList() where user.UserName == userName select user;

            return foundUsers.Count() != 0;
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

            var role = from user in _context.User.ToList() where user.UserName == userName select user.UserRole;

            return role.ElementAt(0);   //should only ever be one user with the same name, but query returns an enum
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


            /// posts debug:
            Models.Artist ar = new Artist(1, "testartist");
            List<Artist> artistArray = new List<Artist>();
            artistArray.Add(ar);
            Models.Song testsong = new Song(2, "title2", 180, "desc", artistArray, new List<Genre>(), new List<Link>());
            newUser.uploadedSongs.Add(testsong);    ///////////////// !!!!!!!!!!!!!!!!!!!!!! debug!!!

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