namespace musicCatalogDotNetAPI.Models
{
    public class Song
    {
        public int SongId { get; set; }

        public string Title { get; set; }

        public int Time { get; set; }   //in seconds, converted into m:ss or up to hh:mm:ss when displaying

        public DateTime CreatedAt { get; set; }

        public string Description { get; set; }



        public Song(int Id, string Title, int Time, string Desc, /*User Uploader,*/ List<Artist> Artists, List<Genre> Genres, List<Link> Links)
        {
            this.SongId = Id;
            this.Title = Title;
            this.Time = Time;
            this.CreatedAt = DateTime.Now;
            this.Description = Desc;

            /*this.UserId = Uploader.UserId;
            this.UploadedBy = Uploader;*/

            this.Artists = Artists;
            this.Genres = Genres;
            this.Links = Links;
        }

        public Song() { }





        public int UserId { get; set; }

        public User UploadedBy { get; set; }





        public List<Artist> Artists { get; /*set;*/ } = new();      //settery do podpinania wartości do artists: [] żeby ich pokazywało wypisując songs
        public List<Genre> Genres { get; } = new();
        public List<Link> Links { get; } = new();


    }
}
