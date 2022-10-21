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
        Models.Song CreateSong(string title, int time, string desc, /*Models.User uploader,*/ string uploaderName/*, List<Artist> artists, List<Genre> genres, List<Link> links*/);  //notice: should be async! Not async for now
    }

    public class SongService : ISongService
    {
        private readonly ILogger<SongService> _logger;
        private readonly DBService _context;

        public SongService(ILogger<SongService> logger, DBService context)
        {
            _logger = logger;
            _context = context;
        }




        ///     !!!     nowe: CreateUserAsync do rejestracji ze stackoverflowa, bo u Changhui'a Xu rejestracji z jakiegoœ powodu nie by³o
        public Models.Song CreateSong(string title, int time, string desc, /*Models.User uploader,*/ string uploaderName/*, List<Artist> artists, List<Genre> genres, List<Link> links*/)
        {
            int newId = _context.Song.Count() + 1;


            //var user = _context.User.Include(b => b.uploadedSongs).Where(b => b.UserId == uploader.UserId);
            User uu = _context.User.Include(b => b.uploadedSongs).Where(b => b.UserName == uploaderName).First();
            User uuuu = _context.User.Where(b => b.UserName == uploaderName).First(); ///dzia³a
            User uuuuuu = _context.User.Include(b => b.uploadedSongs).First();
            var uploader = _context.User.First();
            var u2 = _context.User;
            var u3 = _context.User.Include(b => b.uploadedSongs).Where(b => b.UserName == uploaderName);
            var u4 = from user in _context.User.ToList() where user.UserName == uploaderName select user;

            var u5 = _context.User.Where(b => b.UserName == uploaderName).Include(b => b.uploadedSongs).First();
            var u6 = _context.User.Where(b => b.UserName == uploaderName).Include(b => b.uploadedSongs);

            var u7 = from user in _context.User.ToList() where user.UserName == uploaderName select user;
            var UU = u7.ElementAt(0);

            
            var vvv = new { uu, uuuu, uuuuuu, uploader, u2, u3, u4, u5, u6, u7, UU };
            //var vv = [uu, uuuu, uuuuuu];


            //foreach (Artist artist in artists){}
            List<Artist> artists = new List<Artist>();
            List<Genre> genres = new List<Genre>();
            List<Link> links = new List<Link>();

            Models.Song newSong = new Models.Song(newId,
                                        title,
                                        time,
                                        desc,
                                        //uploader,
                                        artists,
                                        genres,
                                        links);

            //uploader.uploadedSongs.Add(newSong);

            //newSong.UserId = UU.UserId;       //or this?
            //newSong.UploadedBy = UU;// also causes object cycle? I guess these are not meant to go into database

            UU.uploadedSongs.Add(newSong);    //causes object cycle, exceeds max depth of 32, crashes

            _context.Song.Add(newSong);
            _context.SaveChanges();

            return newSong;

        }



    }



}