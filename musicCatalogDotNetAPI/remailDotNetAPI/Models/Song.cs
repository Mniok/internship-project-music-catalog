namespace musicCatalogDotNetAPI.Models
{
    public class Song
    {
        public int SongId { get; set; }

        public string Title { get; set; }

        public int Time { get; set; }   //in seconds, converted into m:ss or up to hh:mm:ss when displaying

        public string Description { get; set; }




        public int UserId { get; set; }

        public User UploadedBy { get; set; }





        public List<Artist> Artists { get; } = new();
        public List<Genre> Genres { get; } = new();
        public List<Link> Links { get; } = new();


    }
}
