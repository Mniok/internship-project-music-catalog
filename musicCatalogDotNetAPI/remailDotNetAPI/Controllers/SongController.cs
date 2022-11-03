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
            var songs = await _context.Song
                .Include(b => b.UploadedBy)
                .Include(b => b.Artists)
                .Include(b => b.Genres)
                .Include(b => b.Links)
                .ToListAsync();
            
            foreach (Song song in songs)
            {
                //song = _songService.CleanupSong(song); //cannot assign to foreach iteration variable
                var cleanSong = _songService.CleanupSong(song);
                song.UploadedBy = cleanSong.UploadedBy;
                song.Artists = cleanSong.Artists;
                song.Genres = cleanSong.Genres;
                song.Links = cleanSong.Links;
            }

            return songs;
        }

        /*/ [Authorize] /*/ [AllowAnonymous] /**/
        [EnableCors]
        [HttpGet("songs/{byMe}/{byTitle}/{byArtists}/{byGenres}/{page}/{pageSize}")]
        public async Task<ActionResult<IEnumerable<Song>>> SearchSongList(bool byMe, string byTitle, string byArtists, string byGenres, int page, int pageSize)
        {
            var UserName = User.Identity?.Name;

            //var songsQuery = await _context.Song
            var songs = await _context.Song
                .Include(b => b.UploadedBy)
                .Include(b => b.Artists)
                .Include(b => b.Genres)
                .Include(b => b.Links)//.AsAsyncEnumerable()
                //.Where(b => b.Title == byTitle)
                .ToListAsync();

            foreach (Song song in songs)
            {
                //song = _songService.CleanupSong(song); //cannot assign to foreach iteration variable
                var cleanSong = _songService.CleanupSong(song);
                song.UploadedBy = cleanSong.UploadedBy;
                song.Artists = cleanSong.Artists;
                song.Genres = cleanSong.Genres;
                song.Links = cleanSong.Links;
            }

            return songs;
        }

        /*/ [Authorize] /*/
        [AllowAnonymous] /**/
        [EnableCors]
        [HttpGet("song/{id}")]
        public async Task<ActionResult<Song>> GetSongById(int id)
        {

            //Song song = _context.Song.Where(b => b.SongId == id).First(); ////nie uwzglednia tych co w tym nizej sa included - sa null i []
            Song song = _context.Song
                .Include(b => b.UploadedBy)
                .Include(b => b.Artists)
                .Include(b => b.Genres)
                .Include(b => b.Links)
                .Where(b => b.SongId == id).First();

            song = _songService.CleanupSong(song);

            return song;
        }

        /*/ [Authorize] /*/ [AllowAnonymous] /**/
        [EnableCors]
        [HttpGet("artists")]
        public async Task<ActionResult<IEnumerable<Artist>>> GetArtistsList()
        {
            var artists = await _context.Artist.ToListAsync();

            return artists;
        }

        /*/ [Authorize] /*/ [AllowAnonymous] /**/
        [EnableCors]
        [HttpGet("genres")]
        public async Task<ActionResult<IEnumerable<Genre>>> GetGenresList()
        {
            var genres = await _context.Genre.ToListAsync();

            return genres;
        }

        /*/ [Authorize] /*/ [AllowAnonymous] /**/
        [EnableCors]
        [HttpGet("links")]
        public async Task<ActionResult<IEnumerable<Link>>> GetLinksList()
        {
            var links = await _context.Link.ToListAsync();

            return links;
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

            var createdSong = _songService.CreateSong(request.Title, request.Time, request.Description, currentUser, request.Artists, request.Genres, request.Links);

            if (createdSong == null)
                return BadRequest(new
                {
                    code = "Upload failed",
                    description = "For some reason the song couldn't be uploaded. Please try again later."
                });



            //var role = _userService.GetUserRole(request.UserName);      // GetUserRole po prostu zwraca UserRoles.BasicUser;

            createdSong = _songService.CleanupSong(createdSong);

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

        [JsonPropertyName("time")]
        public int Time { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }



        //[Required]
        [JsonPropertyName("artists")]
        public List<string> Artists { get; set; }

        //[Required]
        [JsonPropertyName("genres")]
        public List<string> Genres { get; set; }

        //[Required]
        [JsonPropertyName("links")]
        public List<UploadLink> Links { get; set; }

    }

    public class UploadLink
    {
        [Required]
        [JsonPropertyName("ToSite")]
        public string ToSite { get; set; }

        [Required]
        [JsonPropertyName("LinkBody")]
        public string LinkBody { get; set; }

    }

} //end of namespace