namespace musicCatalogDotNetAPI.Models
{
    public class Genre
    {
        public int GenreId { get; set; }

        public string GenreName { get; set; }



        public Genre(int Id, string Name)
        {
            this.GenreId = Id;
            this.GenreName = Name;
        }

        public Genre() { }




        public int SongId { get; set; }

        public Song Song { get; set; }


    }
}
