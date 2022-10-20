namespace musicCatalogDotNetAPI.Models
{
    public class SubEmail
    {
        public int SubEmailId { get; set; }

        public string Email { get; set; }




        public int MasterEmailId { get; set; }

        public MasterEmail MasterEmail { get; set; }

    }
}
