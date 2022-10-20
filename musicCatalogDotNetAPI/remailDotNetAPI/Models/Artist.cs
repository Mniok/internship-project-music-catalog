namespace musicCatalogDotNetAPI.Models
{
    public class Artist
    {
        public int ArtistId { get; set; }

        public string ArtistName { get; set; }




        public int SongId { get; set; }

        public Song Song { get; set; }


    }
}
