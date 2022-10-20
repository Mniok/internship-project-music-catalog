namespace musicCatalogDotNetAPI.Models
{
    public class Genre
    {
        public int GenreId { get; set; }

        public string GenreName { get; set; }




        public int SongId { get; set; }

        public Song Song { get; set; }


    }
}
