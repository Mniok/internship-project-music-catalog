using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using musicCatalogDotNetAPI.Infrastructure;
using musicCatalogDotNetAPI.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using Microsoft.AspNetCore.Cors;
using Microsoft.EntityFrameworkCore;
using musicCatalogDotNetAPI.Models;

namespace musicCatalogDotNetAPI.Controllers
{

    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class SongController : ControllerBase
    {
        private readonly ILogger<SongController> _logger;
        private readonly IUserService _userService;
        private readonly IJwtAuthManager _jwtAuthManager;

        private readonly DBService _context;

        public SongController(ILogger<SongController> logger, IUserService userService, IJwtAuthManager jwtAuthManager, DBService context)
        {
            _logger = logger;
            _userService = userService;
            _jwtAuthManager = jwtAuthManager;
            _context = context;
        }





        ///         TEST ===========================================================================================
        [AllowAnonymous]
        [EnableCors]
        [HttpGet("testController")]
        public ActionResult TestController()
        {
            return Ok(new LoginResult { UserName = "controller test", Role = "test", OriginalUserName = "controller test" });
        }


        [Authorize]
        [EnableCors]
        [HttpGet("songs")]
        public async Task<ActionResult<IEnumerable<Song>>> GetSongList()
        {
            return await _context.Song.ToListAsync();
        }


        [Authorize]
        [EnableCors]
        [HttpPost("registration")]
        public async Task<IActionResult> UploadSong([FromBody] RegisterRequest request)
        {

            string uploader = User.Identity?.ToString();
            
            
            if (!ModelState.IsValid)
                return BadRequest(new { code = "ModelNotValid", description = "Model is not valid." });

            /*if (_userService.IsAnExistingUser(request.UserName))
                return BadRequest(new
                {
                    code = "UserNameTaken",
                    description = $"The User Name {request.UserName} is taken."
                });*/       //check if song is already uploaded? chyba niepotrzebne ale na razie zostawiam snippet

            //_userService.CreateUser(request.UserName, request.Password);
                    /// !!!! var createdUser = _userService.CreateUser(request.UserName, request.Password);

/*            if (createdUser == null)        /// !!! might not actually work to determine whether user creation failed
                return BadRequest(new
                {
                    code = "Register failed",
                    description = "For some reason the account couldn't be created. Try again later."
                });
*/
            /*var user = _userService.GetUserByUserName(model.UserName);        // to z rejestracji ze stackoverflow, podmienione na generacjê tokenów z poradnika Changhui'a Xu
            int validityDurationInHours = 3;
            string token = _jwtService.GenerateJwtToken(user, await _userService.GetUserRolesAsync(user),
                validityDurationInHours);

            return Ok(new { code = "RegistrationSuccess", auth_token = token });*/

            //var role = _userService.GetUserRole(request.UserName);      // GetUserRole po prostu zwraca UserRoles.BasicUser;
 /*           var role = createdUser.UserRole;
            var claims = new[]                                          // (w swaggerze jest jako string "role": "BasicUser")
            {                                                           // adminów nie bêdzie
                new Claim(ClaimTypes.Name, createdUser.UserName),
                new Claim(ClaimTypes.Role, role)
            };

            var jwtResult = _jwtAuthManager.GenerateTokens(createdUser.UserName, claims, DateTime.Now);
            _logger.LogInformation($"User [{createdUser.UserName}] was registered, and logged in the system.");     */
            return Ok(new LoginResult       // od razu zalogowanie z rejestracji
            {
                UserName = "ABCtest",
                Role = "ABCtest",
                AccessToken = "BCDtest",
                RefreshToken = "BCDEFGtest"
            });
        }



    }






    /// klasy

} //end of namespace