using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using musicCatalogDotNetAPI.Models;
using musicCatalogDotNetAPI.Controllers;
///using Microsoft.AspNetCore.Mvc;

namespace musicCatalogDotNetAPI.Services
{
    public interface ISongService
    {
        Models.Song CreateSong(string title, int time, string desc, /*Models.User uploader,*/ string uploaderName, List<string> artists, List<string> genres, List<UploadLink> links);  //notice: should be async! Not async for now
        //Models.Artist CreateArtist(string name);
        Models.Song CleanupSong(Models.Song song);
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

        /*public Models.Artist CreateArtist(string name)
        {
            int newId = _context.Artist.Count() + 1;

            Artist artist = new Artist(newId, name);
            _context.Artist.Add(artist);    //¿eby nastêpny artysta mia³ wy¿sze Id

            return artist;
        }*/


        public Models.Song CreateSong(string title, int time, string desc, /*Models.User uploader,*/ string uploaderName, List<string> artists, List<string> genres, List<UploadLink> links)
        {
            int newId = _context.Song.Count() + 1;


            //var user = _context.User.Include(b => b.uploadedSongs).Where(b => b.UserId == uploader.UserId);       ////
            User uploader = _context.User.Include(b => b.uploadedSongs).Where(b => b.UserName == uploaderName).First();

            List<Artist> artistsList = new List<Artist>();
            List<Genre> genresList = new List<Genre>();
            List<Link> linksList = new List<Link>();

            Models.Song newSong = new Models.Song(newId,
                                        title,
                                        time,
                                        desc,
                                        //uploader,
                                        artistsList,
                                        genresList,
                                        linksList);


            int newArtistId = _context.Artist.Count() + 1;
            foreach (string artist in artists)
            {
                Artist a = new Artist(newArtistId, artist);
                artistsList.Add(a);
                newSong.Artists.Add(a);
                newArtistId++;
            }

            int newGenreId = _context.Genre.Count() + 1;
            foreach (string genre in genres)
            {
                Genre g = new Genre(newGenreId, genre);
                genresList.Add(g);
                newSong.Genres.Add(g);
                newGenreId++;
            }

            int newLinkId = _context.Link.Count() + 1;
            foreach (UploadLink link in links)
            {
                Link l = new Link(newLinkId, link.ToSite, link.LinkBody);
                linksList.Add(l);
                newSong.Links.Add(l);
                newLinkId++;
            }


            //newSong.Artists.Add(artistsList);

            uploader.uploadedSongs.Add(newSong);

            _context.Song.Add(newSong);
            _context.SaveChanges();

            return newSong;

        }

        public Models.Song CleanupSong(Models.Song song)
        {

            song.UploadedBy.Password = "***";
            song.UploadedBy.uploadedSongs.Clear(); //to reduce circular references

            foreach (Artist artist in song.Artists)
                artist.Song = null; //to reduce circular references...

            foreach (Genre genre in song.Genres)
                genre.Song = null;

            foreach (Link link in song.Links)
                link.Song = null;

            return song;
        }


    }

}