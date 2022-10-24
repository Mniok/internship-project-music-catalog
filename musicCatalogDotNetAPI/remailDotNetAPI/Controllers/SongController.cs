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
using NuGet.Protocol;

namespace musicCatalogDotNetAPI.Controllers
{

    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class SongController : ControllerBase
    {
        private readonly ILogger<SongController> _logger;
        //private readonly IUserService _userService;
        private readonly ISongService _songService;
        private readonly IJwtAuthManager _jwtAuthManager;

        private readonly DBService _context;

        public SongController(ILogger<SongController> logger, ISongService songService, IJwtAuthManager jwtAuthManager, DBService context)
        {
            _logger = logger;
            _songService = songService;
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


        /*/ [Authorize] /*/ [AllowAnonymous] /**/
        [EnableCors]
        [HttpGet("songs")]
        public async Task<ActionResult<IEnumerable<Song>>> GetSongList()
        {
            var songs = await _context.Song.ToListAsync();
            
            foreach (Song song in songs)
            {
                var user = from u in _context.User.ToList() where u.UserId == song.UserId select u;
                song.UploadedBy = user.First();

                var artists = from a in _context.Artist.ToList() where a.SongId == song.SongId select a;
                song.Artists = artists.ToList<Artist>();

                var genres = from g in _context.Genre.ToList() where g.SongId == song.SongId select g;
                song.Genres = genres.ToList<Genre>();

                var links = from l in _context.Link.ToList() where l.SongId == song.SongId select l;
                song.Links = links.ToList<Link>();
            }

            return songs;
        }


        [Authorize]
        [EnableCors]
        [HttpPost("upload")]
        public async Task<IActionResult> UploadSong([FromBody] UploadSongRequest request)
        {

            string currentUser = User.Identity?.Name;
            
            
            if (!ModelState.IsValid)
                return BadRequest(new { code = "ModelNotValid", description = "Model is not valid." });

            /*if (_userService.IsAnExistingUser(request.UserName))
                return BadRequest(new
                {
                    code = "UserNameTaken",
                    description = $"The User Name {request.UserName} is taken."
                });*/       //check if song is already uploaded? chyba niepotrzebne ale na razie zostawiam snippet

            //_userService.CreateUser(request.UserName, request.Password);
            ///var createdUser = _userService.CreateUser(request.UserName, request.Password);
            var createdSong = _songService.CreateSong(request.Title, request.Time, request.Description, currentUser/*, request.Artists, request.Genres, request.Links*/);

/*            if (createdUser == null)        /// !!! might not actually work to determine whether user creation failed
                return BadRequest(new
                {
                    code = "Register failed",
                    description = "For some reason the account couldn't be created. Try again later."
                });
*/


            //var role = _userService.GetUserRole(request.UserName);      // GetUserRole po prostu zwraca UserRoles.BasicUser;

            _logger.LogInformation($"Song [{createdSong.Title}] was added by user [{currentUser}].");
            return Ok(createdSong);
        }



    }






    /// klasy

    public class UploadSongRequest
    {
        [Required]
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [Required]
        [JsonPropertyName("time")]
        public int Time { get; set; }

        [Required]
        [JsonPropertyName("description")]
        public string Description { get; set; }



        //[Required]
        [JsonPropertyName("artists")]
        public List<Artist> Artists { get; set; }

        //[Required]
        [JsonPropertyName("genres")]
        public List<Genre> Genres { get; set; }

        //[Required]
        [JsonPropertyName("links")]
        public List<Link> Links { get; set; }

    }

} //end of namespace