namespace musicCatalogDotNetAPI.Models
{
    public class Artist
    {
        public int ArtistId { get; set; }

        public string ArtistName { get; set; }





        public Artist(int Id, string Name)
        {
            this.ArtistId = Id;
            this.ArtistName = Name;
        }

        public Artist() { }






        public int SongId { get; set; }

        public Song Song { get; set; }


    }
}
