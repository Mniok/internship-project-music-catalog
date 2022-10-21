using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using musicCatalogDotNetAPI.Models;
///using Microsoft.AspNetCore.Mvc;

namespace musicCatalogDotNetAPI.Services
{
    public interface ISongService
    {
        Models.Song CreateSong(string title, int time, string desc, Models.User uploader, List<Artist> artists, List<Genre> genres, List<Link> links);  //notice: should be async! Not async for now
    }

    public class SongService : ISongService
    {
        private readonly ILogger<SongService> _logger;
        private readonly DBService _context;


        private readonly IDictionary<string, string> _users = new Dictionary<string, string>            /// !!!!!!!! zast¹piæ database
        {
            { "test1", "password1" },
            { "test2", "password2" },
            { "admin", "securePassword" }
        };
        // inject your database here for user validation
        public SongService(ILogger<SongService> logger, DBService context)
        {
            _logger = logger;
            _context = context;
        }




        ///     !!!     nowe: CreateUserAsync do rejestracji ze stackoverflowa, bo u Changhui'a Xu rejestracji z jakiegoœ powodu nie by³o
        public Models.Song CreateSong(string title, int time, string desc, Models.User uploader, List<Artist> artists, List<Genre> genres, List<Link> links)
        {
            int newId = _context.Song.Count() + 1;

            //foreach (Artist artist in artists){}

            Models.Song newSong = new Models.Song(newId,
                                        title,
                                        time,
                                        desc,
                                        uploader,
                                        artists,
                                        genres,
                                        links);


            //_context.Song.Add(newSong);               ////!!!!!!!!!!!!!! uncomment!
            //await _context.SaveChangesAsync();
            //_context.SaveChanges();               ////!!!!!!!!!!!!!! uncomment!

            //return CreatedAtAction("GetUser", new { id = user.UserId }, user);
            //return new Models.User(1, "testuser1", "testpass1", Services.UserRoles.BasicUser);
            return newSong;

        }



    }



}