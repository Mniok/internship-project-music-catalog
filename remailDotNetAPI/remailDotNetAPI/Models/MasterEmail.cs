namespace remailDotNetAPI.Models
{
    public class MasterEmail
    {
        public int MasterEmailId { get; set; }

        public string Email { get; set; }




        public int UserId { get; set; }

        public User User { get; set; }





        public List<SubEmail> subEmails { get; } = new();

    }
}
